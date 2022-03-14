using AutoMapper;
using Reports.Application.Responses;
using Reports.Domain.Entities;

namespace Reports.Application.Mappers
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportResponse>().ReverseMap();
        }
    }
}
