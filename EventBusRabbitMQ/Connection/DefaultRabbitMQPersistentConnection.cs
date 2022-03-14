using EventBusRabbitMQ.Connection.Abstract;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace EventBusRabbitMQ.Connection
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        #region Variables
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private bool _disposed;
        #endregion

        public DefaultRabbitMQPersistentConnection(
            IConnectionFactory connectionFactory,
            ILogger<DefaultRabbitMQPersistentConnection> logger,
            int retryCount)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            var policy = CreatePolicy();

            policy.Execute(() => {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                HandleConnectionEvents();
                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connectionFactory.VirtualHost);

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        #region TryConnectHelperMethods
        private RetryPolicy CreatePolicy()
        {
            return RetryPolicy
                            .Handle<SocketException>()
                            .Or<BrokerUnreachableException>()
                            .WaitAndRetry(_retryCount,
                                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                            (ex, time) =>
                                            {
                                                _logger.LogWarning(ex, "RabbitMQ Client could not connect affter {TimeOut}s ({ExceptionMessage})",
                                                       $"{time.TotalSeconds:n1}",
                                                       ex.ToString());
                                            });
        }
        private void HandleConnectionEvents()
        {
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.ConnectionBlocked += OnConnectionBlocked;
            _connection.CallbackException += OnCallbackException;
        }
        #endregion

        #region RabbitMQ Connection Events
        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }
        #endregion
    }
}
