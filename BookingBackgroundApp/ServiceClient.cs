using Application._BookedTicket;
using Infrastructure;

namespace BookingBackgroundApp
{
    public class ServiceClient
    {
        private readonly IBookedTicketService _ticketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ServiceClient> _logger;

        public ServiceClient(IBookedTicketService ticketService, IUnitOfWork unitOfWork, ILogger<ServiceClient> logger)
        {
            _ticketService = ticketService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task RemoveExpiredBookings(CancellationToken cancellationToken=default)
        {
            await _ticketService.RemoveExpiredBookedTickets(cancellationToken);
        }
    }
}
