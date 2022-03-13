using AutoMapper;
using Contacts.Application.Commands.UpdateContact;
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
    class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ContactResponse>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public UpdateContactHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ContactResponse> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contactEntity = _mapper.Map<Contact>(request);

            if (contactEntity == null)
            {
                throw new ApplicationException(ErrorMessages.NOT_MAPPED);
            }

            var isSuccesful = await _repo.Update(contactEntity);

            var response = isSuccesful ? _mapper.Map<ContactResponse>(contactEntity) : null;

            return response;
        }
    }
}
