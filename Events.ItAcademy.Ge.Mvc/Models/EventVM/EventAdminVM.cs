using Application._Event.Models.Request;
using Application._Event.Models.Response;

namespace Events.ItAcademy.Ge.Mvc.Models.EventVM
{
    public class EventAdminVM
    {
        public EventResponseModel Event { get; set; }
        public EventConfirmationModel Request { get; set; }
    }
}
