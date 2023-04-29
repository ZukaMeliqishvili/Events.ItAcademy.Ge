using Application._Event.Models.Request;
using Application._Event.Models.Response;
using Application._Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.It.Academy.Ge.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        protected string GetUserId() => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        /// <summary>
        /// get list of confirmed events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
        [HttpGet("confirmedEvents")]
        public async Task<List<EventResponseModel>> GetAllConfirmedEvent(
          CancellationToken cancellationToken)
        {
            return await _eventService.GetAllConfirmedEventsAsync(cancellationToken);
        }

        /// <summary>
        /// get confirmed event by id
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
        [HttpGet("confirmedEvents/{id}")]
        public async Task<EventResponseModel> GetConfirmedEvent(
            int id, CancellationToken cancellationToken)
        {
            return await _eventService.GetConfirmedEventByIdAsync(id, cancellationToken);

        }
        /// <summary>
        /// get events wich belong to user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("userEvents")]

        public async Task<List<EventResponseModel>> GetAllUserEvents(CancellationToken cancellationToken)
        {
            return await _eventService.GetAllUserEventAsync(cancellationToken, GetUserId());
        }

        /// <summary>
        /// get event by id which belong to user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("userEvents/{id}")]
        public async Task<EventResponseModel> GetUserEvent(
          int id,
          CancellationToken cancellationToken)
        {
            return await _eventService.GetUserEventAsync(id, cancellationToken, GetUserId());
        }
        /// <summary>
        /// Create new event
        /// </summary>
        /// <param name="cancellationToken"></param>
        ///<param name="event"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EventRequestModel), StatusCodes.Status200OK)]
        [Authorize]
        //[MapToApiVersion("1.0")]
        [HttpPost]

        public async Task Create( EventRequestModel @event, CancellationToken cancellationToken)
        {
            await _eventService.CreateAsync(@event, GetUserId(), cancellationToken);
        }
        
        /// <summary>
        /// Update Event
        /// </summary>
        /// <param name="cancellationToken"></param>
        ///<param name="event"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task Update(
          EventUpdateRequestModel @event,
          int id,CancellationToken cancellationToken)
        {
            await _eventService.UpdateEventAsync(@event, id, GetUserId(), cancellationToken);
        }
    }
}
