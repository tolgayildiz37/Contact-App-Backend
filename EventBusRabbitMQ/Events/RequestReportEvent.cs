using EventBusRabbitMQ.Events.Abstract;
using System;
using System.Collections.Generic;

namespace EventBusRabbitMQ.Events
{
    public class RequestReportEvent : IEvent
    {
        public int Status { get; set; }
        public List<ReportDataEvent> ReportDatas { get; set; }
    }
}
