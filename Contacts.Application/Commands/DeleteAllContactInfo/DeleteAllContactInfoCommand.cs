using Contacts.Application.Responses;
using MediatR;

namespace Contacts.Application.Commands.DeleteAllContactInfo
{
    public class DeleteAllContactInfoCommand : IRequest<ResponseCQRS>
    {
        public string Id { get; set; }
    }
}
