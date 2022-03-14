using AutoMapper;
using MediatR;
using Reports.Application.Commands.AddReport;
using Reports.Application.Constants;
using Reports.Application.Responses;
using Reports.Domain.Entities;
using Reports.Domain.Repositories.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reports.Application.Handlers
{
    public class AddReportHandler : IRequestHandler<AddReportCommand, ReportResponse>
    {
        private readonly IReportRepository _repo;
        private readonly IMapper _mapper;

        public AddReportHandler(IReportRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReportResponse> Handle(AddReportCommand request, CancellationToken cancellationToken)
        {
            var reportEntity = _mapper.Map<Report>(request);

            if (reportEntity == null)
            {
                throw new ApplicationException(ErrorMessages.NOT_MAPPED);
            }

            reportEntity.CompleteDate = DateTime.UtcNow;
            reportEntity.Status = (int)ReportConstants.Status.Completed;

            var result = await _repo.Add(reportEntity);

            var response = _mapper.Map<ReportResponse>(result);

            return response;
        }
    }
}
