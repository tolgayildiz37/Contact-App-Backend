using AutoMapper;
using Contacts.Application.Commands.AddContact;
using Contacts.Application.Constants;
using Contacts.Application.Responses;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories.Abstract;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Application.Handlers
{
    public class AddContactHandler : IRequestHandler<AddContactCommand, ContactResponse>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public AddContactHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ContactResponse> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            var contactEntity = _mapper.Map<Contact>(request);

            if(contactEntity == null)
            {
                throw new ApplicationException(ErrorMessages.NOT_MAPPED);
            }

            var contact = await _repo.Add(contactEntity);

            var response = _mapper.Map<ContactResponse>(contact);

            return response;
        }
    }
}
