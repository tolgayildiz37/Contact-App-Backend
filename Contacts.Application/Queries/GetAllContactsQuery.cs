using Contacts.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Contacts.Application.Queries
{
    public class GetAllContactsQuery : IRequest<IEnumerable<ContactResponse>>
    {

    }
}
