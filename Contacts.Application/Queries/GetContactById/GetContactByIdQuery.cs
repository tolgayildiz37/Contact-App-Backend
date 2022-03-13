using Contacts.Application.Responses;
using MediatR;

namespace Contacts.Application.Queries.GetContactById
{
    public class GetContactByIdQuery : IRequest<ContactResponse>
    {
        public string Id { get; set; }

        public GetContactByIdQuery(string ıd)
        {
            Id = ıd;
        }
    }
}
