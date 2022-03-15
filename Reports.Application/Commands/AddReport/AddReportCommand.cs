using MediatR;
using Reports.Application.Responses;
using System;
using System.Collections.Generic;

namespace Reports.Application.Commands.AddReport
{
    public class AddReportCommand : IRequest<ReportResponse>
    {
        public int Status { get; set; }
        public List<ReportDataResponse> ReportDatas { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
