using AutoMapper;
using MediatR;
using Reports.Application.Queries.GetReportById;
using Reports.Application.Responses;
using Reports.Domain.Repositories.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.Application.Handlers
{
    public class GetReportByIdHandler : IRequestHandler<GetReportByIdQuery, ReportResponse>
    {
        private readonly IReportRepository _repo;
        private readonly IMapper _mapper;

        public GetReportByIdHandler(
            IReportRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ReportResponse> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await _repo.Get(x=> x.Id.Equals(request.Id));

            var response = _mapper.Map<ReportResponse>(report);

            return response;
        }
    }
}
