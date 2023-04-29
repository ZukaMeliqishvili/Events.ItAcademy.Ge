using Application._BookedTicket;
using Application._Event;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventArchivingBackgroundApp
{
    public class ServiceClient
    {
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ServiceClient> _logger;
        private readonly IBookedTicketService _bookedTicketService;

        public ServiceClient(IEventService eventService, IUnitOfWork unitOfWork, ILogger<ServiceClient> logger, IBookedTicketService bookedTicketService)
        {
            _eventService = eventService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _bookedTicketService = bookedTicketService;
        }

        public async Task RemoveExpiredBookings(CancellationToken cancellationToken = default)
        {
            await _eventService.MoveEventsToArchive(cancellationToken);
        }
    }
}
