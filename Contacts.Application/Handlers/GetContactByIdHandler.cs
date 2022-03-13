using AutoMapper;
using Contacts.Application.Queries.GetContactById;
using Contacts.Application.Responses;
using Contacts.Domain.Repositories.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Application.Handlers
{
    public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactResponse>
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;

        public GetContactByIdHandler(
            IContactRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ContactResponse> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _repo.Get(x => x.Id.Equals(request.Id));

            var response = _mapper.Map<ContactResponse>(contact);

            return response;
        }
    }
}
