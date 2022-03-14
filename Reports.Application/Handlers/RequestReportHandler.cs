using AutoMapper;
using MediatR;
using Reports.Application.Constants;
using Reports.Application.Helpers;
using Reports.Application.Queries.RequestReport;
using Reports.Application.Responses;
using Reports.Domain.Repositories.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.Application.Handlers
{
    public class RequestReportHandler : IRequestHandler<RequestReportQuery, ReportResponse>
    {
        private readonly IReportRepository _repo;
        private readonly IMapper _mapper;

        public RequestReportHandler(IReportRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReportResponse> Handle(RequestReportQuery request, CancellationToken cancellationToken)
        {
            var newReport = ReportHelper.CreateNewReport();
            await _repo.Add(newReport);

            if(newReport == null || String.IsNullOrWhiteSpace(newReport.Id))
            {
                throw new ApplicationException(ErrorMessages.REPORT_CREATE_ERROR);
            }

            var response = _mapper.Map<ReportResponse>(newReport);

            return response;
        }
    }
}
