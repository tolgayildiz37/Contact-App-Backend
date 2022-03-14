using AutoMapper;
using Contacts.Application.Responses;
using EventBusRabbitMQ.Events;

namespace Contacts.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ReportResponse, RequestReportEvent>().ReverseMap();
            CreateMap<ReportDataResponse, ReportDataEvent>().ReverseMap();
        }
    }
}
