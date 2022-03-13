using Contacts.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Contacts.Application.Queries.GetAllContacts
{
    public class GetAllContactsQuery : IRequest<IEnumerable<ContactResponse>>
    {

    }
}
