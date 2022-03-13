using Contacts.Application.Commands.Abstract;
using Contacts.Application.Responses;
using MediatR;

namespace Contacts.Application.Commands.UpdateContact
{
    public class UpdateContactCommand : ContactCommand, IRequest<ContactResponse>
    {
        public string Id { get; set; }
    }
}
