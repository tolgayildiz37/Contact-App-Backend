using MediatR;
using Reports.Application.Responses;

namespace Reports.Application.Queries.GetReportById
{
    public class GetReportByIdQuery : IRequest<ReportResponse>
    {
        public string Id { get; set; }

        public GetReportByIdQuery(string id)
        {
            Id = id;
        }
    }
}
