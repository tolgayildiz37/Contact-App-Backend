using Contacts.Application.Commands.Abstract;
using Contacts.Application.Responses;
using MediatR;

namespace Contacts.Application.Commands.AddContact
{
    public class AddContactCommand : ContactCommand, IRequest<ContactResponse>
    {
        
    }
}
