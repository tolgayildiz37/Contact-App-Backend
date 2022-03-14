namespace EventBusRabbitMQ.Consumer
{
    public interface IConsumerBase
    {
        void Consume();
        void Disconnect();
    }
}
