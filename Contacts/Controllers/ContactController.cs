using Contacts.Application.Commands.AddContact;
using Contacts.Application.Queries;
using Contacts.Application.Responses;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Contacts.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IMediator mediator, ILogger<ContactController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> GetContacts()
        {
            var query = new GetAllContactsQuery();

            var contacts = await _mediator.Send(query);

            if (contacts.Count() == decimal.Zero)
                return NotFound();

            return Ok(contacts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<ContactResponse>> AddContact([FromBody] AddContactCommand contact)
        {
            var result = await _mediator.Send(contact);
            return Ok(result);
        }
    }
}
