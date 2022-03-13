using Contacts.Application.Commands.Abstract;
using Contacts.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Contacts.Application.Commands.AddContactInfo
{
    public class AddContactInfoCommand : IRequest<ResponseCQRS>
    {
        public string Id { get; set; }
        public List<ContactInfoCommand> ContactInformation { get; set; }
    }
}
