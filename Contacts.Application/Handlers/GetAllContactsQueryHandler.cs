using AutoMapper;
using Contacts.Application.Queries;
using Contacts.Application.Responses;
using Contacts.Domain.Repositories.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Application.Handlers
{
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactResponse>>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public GetAllContactsQueryHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactResponse>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contactList = await _repo.GetAll();

            var response = _mapper.Map<IEnumerable<ContactResponse>>(contactList);

            return response; ;
        }
    }
}
