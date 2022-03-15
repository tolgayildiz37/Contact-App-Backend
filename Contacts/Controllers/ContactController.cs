using AutoMapper;
using Contacts.Application.Commands.AddContact;
using Contacts.Application.Commands.AddContactInfo;
using Contacts.Application.Commands.DeleteAllContactInfo;
using Contacts.Application.Commands.DeleteContact;
using Contacts.Application.Commands.DeleteContactInfo;
using Contacts.Application.Commands.UpdateContact;
using Contacts.Application.Constants;
using Contacts.Application.Queries.GetAllContacts;
using Contacts.Application.Queries.GetContactById;
using Contacts.Application.Queries.RequestReport;
using Contacts.Application.Responses;
using EventBusRabbitMQ.Constants;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producers;
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
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;

        public ContactController(
            IMapper mapper,
            IMediator mediator, 
            ILogger<ContactController> logger,
            EventBusRabbitMQProducer eventBus)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ContactResponse>>> GetContacts()
        {
            var query = new GetAllContactsQuery();

            var contacts = await _mediator.Send(query);

            if (contacts.Count() == decimal.Zero)
            {
                _logger.LogError(ErrorMessages.COLLECTION_EMPTY);
                return NotFound();
            }

            return Ok(contacts);
        }

        [HttpGet("{id:length(24)}", Name = "GetContact")]
        [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ContactResponse>> GetContact(string id)
        {
            var query = new GetContactByIdQuery(id);
            var contact = await _mediator.Send(query);

            if (contact == null)
            {
                _logger.LogError($"{ErrorMessages.NOT_FOUND_IN_COLLECTION} Contact Id: {id}");
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpGet("RequestReport")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddReport()
        {
            var query = new RequestReportQuery();

            var response = await _mediator.Send(query);

            var eventMessage = _mapper.Map<RequestReportEvent>(response);

            try
            {
                _eventBus.Publish(EventBusConstants.RequestReportQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.RequestId, "Contact API");
                throw;
            }

            return Accepted();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContactResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<ContactResponse>> AddContact([FromBody] AddContactCommand contact)
        {
            var result = await _mediator.Send(contact);
            return Ok(result);
        }

        [HttpPost("AddContactInformation")]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponseCQRS>> AddInformation([FromBody] AddContactInfoCommand contact)
        {
            var result = await _mediator.Send(contact);

            if (result == null || !result.IsSuccessful)
            {
                _logger.LogError($"{ErrorMessages.ADD_ERROR} ContactInformation Parent Id: {contact.Id}");
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ContactResponse>> UpdateContact([FromBody] UpdateContactCommand contact)
        {
            var result = await _mediator.Send(contact);

            if (result == null)
            {
                _logger.LogError($"{ErrorMessages.ADD_ERROR} Contact Id: {contact.Id}");
                return BadRequest();
            }

            return Ok(contact);
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponseCQRS>> DeleteContact(string id)
        {
            var contact = new DeleteContactCommand() { Id = id };
            var result = await _mediator.Send(contact);

            if (result == null || !result.IsSuccessful)
            {
                _logger.LogError($"{ErrorMessages.DELETE_ERROR} Contact Id: {contact.Id}");
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete("DeleteContactInfo")]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponseCQRS>> DeleteContactInfo([FromBody] DeleteContactInfoCommand contact)
        {
            var result = await _mediator.Send(contact);

            if (result == null || !result.IsSuccessful)
            {
                _logger.LogError($"{ErrorMessages.DELETE_ERROR} Contact Information Parent Id: {contact.Id}");
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("DeleteAllContactInfo/{id:length(24)}")]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseCQRS), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponseCQRS>> DeleteAllContactInfo(string id)
        {
            var contact = new DeleteAllContactInfoCommand() { Id = id };
            var result = await _mediator.Send(contact);

            if (result == null || !result.IsSuccessful)
            {
                _logger.LogError($"{ErrorMessages.DELETE_ERROR} Contact Information Parent Id: {contact.Id}");
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
