using AutoMapper;
using Contacts.Application.Constants;
using Contacts.Application.Queries.RequestReport;
using Contacts.Application.Responses;
using Contacts.Domain.Repositories.Abstract;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Application.Handlers
{
    public class RequestReportHandler : IRequestHandler<RequestReportQuery, ReportResponse>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public RequestReportHandler(IContactRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReportResponse> Handle(RequestReportQuery request, CancellationToken cancellationToken)
        {
            //Db'den rapor çekilecek
            var contacts = await _repo.GetAll();

            var response = new ReportResponse()
            {
                Status = (int)ReportConstants.Status.Preparing,
                CreateTime = DateTime.UtcNow,
                ReportDatas = null
            };

            return response;
        }
    }
}
