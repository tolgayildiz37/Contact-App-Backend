namespace EventBusRabbitMQ.Events
{
    public class ReportDataEvent
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
