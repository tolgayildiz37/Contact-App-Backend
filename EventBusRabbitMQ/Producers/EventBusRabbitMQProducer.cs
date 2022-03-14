using EventBusRabbitMQ.Connection.Abstract;
using EventBusRabbitMQ.Events.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace EventBusRabbitMQ.Producers
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQProducer> _logger;
        private readonly int _retryCount;

        public EventBusRabbitMQProducer(IRabbitMQPersistentConnection persistentConnection,
            ILogger<EventBusRabbitMQProducer> logger,
            int retryCount = 5)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }

        public void Publish(string queueName, IEvent @event)
        {
            TryConnect();

            var policy = CreateRetryPolicy(@event);

            using (var channel = _persistentConnection.CreateModel())
            {
                var body = CreateBody(queueName, channel, @event);

                policy.Execute(() =>
                {
                    channel.ConfirmSelect();
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        mandatory: true,
                        basicProperties: CreateBasicProperties(channel),
                        body: body
                        );
                    channel.WaitForConfirmsOrDie();

                    channel.BasicAcks += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");
                    };
                });
            }
        }

        private void TryConnect()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();
        }

        private RetryPolicy CreateRetryPolicy(IEvent @event)
        {
            return RetryPolicy.Handle<BrokerUnreachableException>()
                                    .Or<SocketException>()
                                    .WaitAndRetry(_retryCount,
                                                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                    (ex, time) =>
                                                    {
                                                        _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.RequestId, $"{time.TotalSeconds:n1}", ex.ToString());
                                                    });
        }

        private byte[] CreateBody(string queueName, IModel channel, IEvent @event)
        {
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = JsonConvert.SerializeObject(@event);
            return Encoding.UTF8.GetBytes(message);
        }

        private IBasicProperties CreateBasicProperties(IModel channel)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            return properties;
        }
    }
}
