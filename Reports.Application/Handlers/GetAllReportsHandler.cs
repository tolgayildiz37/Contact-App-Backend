using AutoMapper;
using MediatR;
using Reports.Application.Queries.GetAllReports;
using Reports.Application.Responses;
using Reports.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.Application.Handlers
{
    class GetAllReportsHandler : IRequestHandler<GetAllReportsQuery, IEnumerable<ReportResponse>>
    {
        private readonly IReportRepository _repo;
        private readonly IMapper _mapper;

        public GetAllReportsHandler(
            IReportRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportResponse>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
        {
            var reportList = await _repo.GetAll();

            var response = _mapper.Map<IEnumerable<ReportResponse>>(reportList);

            return response;
        }
    }
}
