using MediatR;
using Reports.Application.Responses;
using System.Collections.Generic;

namespace Reports.Application.Queries.GetAllReports
{
    public class GetAllReportsQuery : IRequest<IEnumerable<ReportResponse>>
    {

    }
}
