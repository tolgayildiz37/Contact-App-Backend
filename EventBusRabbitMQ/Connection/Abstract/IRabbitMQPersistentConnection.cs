using RabbitMQ.Client;
using System;

namespace EventBusRabbitMQ.Connection.Abstract
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
