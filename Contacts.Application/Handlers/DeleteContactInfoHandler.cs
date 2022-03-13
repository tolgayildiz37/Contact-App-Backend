using AutoMapper;
using Contacts.Application.Commands.DeleteContactInfo;
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
    class DeleteContactInfoHandler : IRequestHandler<DeleteContactInfoCommand, ResponseCQRS>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public DeleteContactInfoHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseCQRS> Handle(DeleteContactInfoCommand request, CancellationToken cancellationToken)
        {
            var contactEntity = _mapper.Map<Contact>(request);

            if (contactEntity == null)
            {
                throw new ApplicationException(ErrorMessages.NOT_MAPPED);
            }

            var isSuccesful = await _repo.DeleteInformation(contactEntity);

            var response = new ResponseCQRS()
            {
                IsSuccessful = isSuccesful,
                Message = $"{InformationMessages.DELETED_SUCCESSFULY} Contact Info Parent Id: {request.Id}"
            };

            return response;
        }
    }
}
