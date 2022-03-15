using Contacts.Application.Commands.Abstract;
using Contacts.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Contacts.Application.Commands.DeleteContactInfo
{
    public class DeleteContactInfoCommand : IRequest<ResponseCQRS>
    {
        public string Id { get; set; }
        public List<ContactInfoCommand> ContactInformation { get; set; }
    }
}
