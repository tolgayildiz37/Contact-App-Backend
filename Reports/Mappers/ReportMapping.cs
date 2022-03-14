using AutoMapper;
using EventBusRabbitMQ.Events;
using Reports.Application.Commands.AddReport;
using Reports.Application.Responses;
using Reports.Domain.Entities;

namespace Reports.Mappers
{
    public class ReportMapping : Profile
    {
        public ReportMapping()
        {
            CreateMap<Report, AddReportCommand>().ReverseMap();
            CreateMap<ReportData, ReportDataResponse>().ReverseMap();
            CreateMap<ReportDataEvent, ReportDataResponse>().ReverseMap();
            CreateMap<ReportStatus, ReportStatusResponse>().ReverseMap();
            CreateMap<RequestReportEvent, ReportResponse>().ReverseMap();
            CreateMap<RequestReportEvent, AddReportCommand>().ReverseMap();
        }
    }
}
