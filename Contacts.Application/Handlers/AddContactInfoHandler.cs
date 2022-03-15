using AutoMapper;
using Contacts.Application.Commands.AddContactInfo;
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
    public class AddContactInfoHandler : IRequestHandler<AddContactInfoCommand, ResponseCQRS>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public AddContactInfoHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        
        public async Task<ResponseCQRS> Handle(AddContactInfoCommand request, CancellationToken cancellationToken)
        {
            var contactInfoEntity = _mapper.Map<Contact>(request);

            if (contactInfoEntity == null)
            {
                throw new ApplicationException(ErrorMessages.NOT_MAPPED);
            }

            var isSuccessful = await _repo.AddInformation(contactInfoEntity);

            var response = new ResponseCQRS()
            {
                IsSuccessful = isSuccessful,
                Message = string.Empty
            };

            return response;
        }
    }
}
