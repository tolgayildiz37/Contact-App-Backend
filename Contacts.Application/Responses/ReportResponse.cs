using System;
using System.Collections.Generic;

namespace Contacts.Application.Responses
{
    public class ReportResponse
    {
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public List<ReportDataResponse> ReportDatas { get; set; }
    }
}
