using System;

namespace EventBusRabbitMQ.Events.Abstract
{
    public abstract class IEvent
    {
        public Guid RequestId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }

        protected IEvent()
        {
            RequestId = Guid.NewGuid();
            CreateTime = DateTime.UtcNow;
        }
    }
}
