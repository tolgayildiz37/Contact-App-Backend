using System;
using System.Collections.Generic;

namespace Reports.Application.Responses
{
    public class ReportResponse
    {
        public string Id { get; protected set; }
        public DateTime CreateTime { get; set; }
        public string Status { get; set; }
        public List<ReportDataResponse> ReportDatas { get; set; }
    }
}
