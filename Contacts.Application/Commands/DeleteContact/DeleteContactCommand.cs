using Contacts.Application.Responses;
using MediatR;

namespace Contacts.Application.Commands.DeleteContact
{
    public class DeleteContactCommand : IRequest<ResponseCQRS>
    {
        public string Id { get; set; }
    }
}
