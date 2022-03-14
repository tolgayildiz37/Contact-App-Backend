using AutoMapper;
using EventBusRabbitMQ.Events;
using Reports.Application.Responses;

namespace Reports.Mappers
{
    public class ReportMapping : Profile
    {
        public ReportMapping()
        {
            CreateMap<RequestReportEvent, ReportResponse>().ReverseMap();
        }
    }
}
