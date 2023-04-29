using Application._Event;
using Application._Event.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Events.It.Academy.Ge.Api.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    

    public class EventController : ControllerBase
    {
        
        private IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        protected string GetUserId() => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        [ProducesResponseType(typeof(EventRequestModel), StatusCodes.Status200OK)]
        [Authorize]
        //[MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<bool> Create(CancellationToken cancellationToken, EventRequestModel @event)
        {
            try
            {
                await _eventService.CreateAsync(@event, GetUserId(), cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
