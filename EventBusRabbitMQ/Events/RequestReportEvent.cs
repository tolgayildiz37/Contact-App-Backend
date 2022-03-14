using EventBusRabbitMQ.Events.Abstract;
using System;

namespace EventBusRabbitMQ.Events
{
    public class RequestReportEvent : IEvent
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime ReportCreateTime { get; set; }
    }
}
