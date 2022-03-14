using System;

namespace Reports.Application.Responses
{
    public class ReportResponse
    {
        public string Id { get; protected set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
    }
}
