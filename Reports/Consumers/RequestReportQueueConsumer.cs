using AutoMapper;
using EventBusRabbitMQ.Connection.Abstract;
using EventBusRabbitMQ.Constants;
using EventBusRabbitMQ.Consumer;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Reports.Application.Commands.AddReport;
using System;
using System.Text;

namespace Reports.Consumers
{
    public class RequestReportQueueConsumer : IConsumerBase
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RequestReportQueueConsumer(
            IRabbitMQPersistentConnection persistentConnection, 
            IMediator mediator, 
            IMapper mapper)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void Consume()
        {
            _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(
                queue: EventBusConstants.RequestReportQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(
                queue: EventBusConstants.RequestReportQueue,
                autoAck: true,
                consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<RequestReportEvent>(message);

            if (e.RoutingKey == EventBusConstants.RequestReportQueue)
            {
                var command = _mapper.Map<AddReportCommand>(@event);

                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
