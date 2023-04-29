using Application._BookedTicket.Models.Request;
using Domain.BoockedTicket;
using Infrastructure;
using Mapster;

namespace Application._BookedTicket
{
    public class BookedTicketService:IBookedTicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookedTicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task CreateAsync( BookedTicketRequestModel entity,CancellationToken cancellationToken)
        {
            var bookedTicket = entity.Adapt<BookedTicket>();
            await _unitOfWork.BookedTicket.CreateAsync(cancellationToken, bookedTicket);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _unitOfWork.BookedTicket.DeleteAsync(cancellationToken,id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<BookedTicket>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<BookedTicket> allAsync = await this._unitOfWork.BookedTicket.GetAllAsync(cancellationToken);
            return allAsync;
        }

        public async Task RemoveExpiredBookedTickets(CancellationToken cancellationToken)
        {
            List<BookedTicket> expiredBookings = await _unitOfWork.BookedTicket.GetAllExpiredAsync(cancellationToken);
            foreach(var book in expiredBookings)
            { 
                await UpdateNumberOfTickets(book, cancellationToken);
            }    
            _unitOfWork.BookedTicket.RemoveRange(expiredBookings);
            await _unitOfWork.SaveAsync();
        }

        private async Task UpdateNumberOfTickets(BookedTicket entity,CancellationToken cancellationToken)
        {
            var @event = await _unitOfWork.Event.GetByIdAsync(cancellationToken, entity.EventId);
            ++@event.NumberOfTickets;
        }
    }
}
