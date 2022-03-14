﻿using AutoMapper;
using Contacts.Application.Constants;
using EventBusRabbitMQ.Constants;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reports.Application.Queries.GetAllReports;
using Reports.Application.Queries.GetReportById;
using Reports.Application.Queries.RequestReport;
using Reports.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Reports.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ReportController> _logger;
        private readonly EventBusRabbitMQProducer _eventBus;

        public ReportController(
            IMapper mapper,
            IMediator mediator, 
            ILogger<ReportController> logger, 
            EventBusRabbitMQProducer eventBus)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReportResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ReportResponse>>> GetReports()
        {
            var query = new GetAllReportsQuery();

            var reportList = await _mediator.Send(query);

            if (reportList.Count() == decimal.Zero)
            {
                _logger.LogError(ErrorMessages.COLLECTION_EMPTY);
                return NotFound();
            }

            return Ok(reportList);
        }

        [HttpGet("{id:length(24)}", Name = "GetReport")]
        [ProducesResponseType(typeof(ReportResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReportResponse>> GetReport(string id)
        {
            var query = new GetReportByIdQuery(id);
            var report = await _mediator.Send(query);

            if (report == null)
            {
                _logger.LogError($"{ErrorMessages.NOT_FOUND_IN_COLLECTION} Report Id: {id}");
                return NotFound();
            }

            return Ok(report);
        }

        [HttpGet("RequestReport")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddReport()
        {
            var query = new RequestReportQuery();
            var report = await _mediator.Send(query);

            var eventMessage = _mapper.Map<RequestReportEvent>(report);

            try
            {
                _eventBus.Publish(EventBusConstants.RequestReportQueue, eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Report API");
                throw;
            }

            return Accepted();
        }
    }
}
