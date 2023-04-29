using Application._BookedTicket;
using Application._Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Events.ItAcademy.Ge.Mvc.Controllers
{

    [Authorize]
    public class TicketController : Controller
    {
        private readonly IEventService _eventService;
        private readonly string userId;
        private readonly IHttpContextAccessor _contextAccessor;
        public TicketController(IEventService eventService,IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
            _eventService = eventService;
            userId= _contextAccessor.HttpContext.User.Claims.Single(x=>x.Type==ClaimTypes.NameIdentifier).Value;
        }
        public async Task<IActionResult> Buy(int id, CancellationToken cancellation)
        {
            try
            {
                await _eventService.BuyTicket(id, userId, cancellation);
                TempData["success"] = "Ticket has been bought successfully";
                return RedirectToAction("Details", "Home", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Details", "Home");
            }
        }
        public async Task<IActionResult> Book(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _eventService.BookTicket(id, userId, cancellationToken);
                TempData["success"] = "Ticket has been booked successfully";

                return RedirectToAction("Details", "Home", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Details", "Home", new { id = id });
            }
        }
    }
}
