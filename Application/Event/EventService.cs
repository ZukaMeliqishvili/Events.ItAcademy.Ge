using Application._BookedTicket;
using Application._BookedTicket.Models.Request;
using Application._Event.Models.Request;
using Application._Event.Models.Response;
using Application.CustomExceptions;
using Application.Localization;
using Domain._Event;
using Domain.BoockedTicket;
using Infrastructure;
using Mapster;
namespace Application._Event
{
    public class EventService:IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookedTicketService _bookedTicketsService;

        public EventService(IUnitOfWork unitOfWork, IBookedTicketService bookedTicketsService)
        {
            _unitOfWork = unitOfWork;
            _bookedTicketsService = bookedTicketsService;
        }

        public async Task CreateAsync(EventRequestModel @event, string userId, CancellationToken cancellationToken)
        {
            if (@event == null)
                throw new ArgumentNullException(ErrorMessages.EmptyObject);
            if (@event.StartDate > @event.EndDate)
                throw new InvalidRequestException(ErrorMessages.StartdateMustBeHigher);
            Event entity = @event.Adapt<Event>();
            entity.UserId = userId;
            await _unitOfWork.Event.CreateAsync(cancellationToken, entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<EventResponseModel>> GetAllConfirmedEventsAsync(
          CancellationToken cancellationToken)
        {
            List<Event> events = await this._unitOfWork.Event.GetAllActiveEvents(cancellationToken);
            List<EventResponseModel> confirmedEventsAsync = events.Adapt<List<EventResponseModel>>();
            return confirmedEventsAsync;
        }

        public async Task<List<EventResponseModel>> GetAllUnconfirmedEventsAsyn(
          CancellationToken cancellationToken)
        {
            List<Event> events = await this._unitOfWork.Event.GetAllUnconfirmedEvents(cancellationToken);
            List<EventResponseModel> unconfirmedEventsAsyn = events.Adapt<List<EventResponseModel>>();
            return unconfirmedEventsAsyn;
        }

        public async Task<List<EventResponseModel>> GetAllUserEventAsync(CancellationToken cancellationToken,string userId)
        {
            List<Event> events = await this._unitOfWork.Event.GetAllEventsByUser(cancellationToken, userId);
            List<EventResponseModel> allUserEventAsync = events.Adapt<List<EventResponseModel>>();

            return allUserEventAsync;
        }
        public async Task Delete(CancellationToken cancellation,int id)
        {
            var obj = await _unitOfWork.Event.GetByIdAsync(cancellation, id);
            if(obj is null)
            {
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            }
            if(obj.IsConfirmed)
            {
                throw new EventIsAlreadyActiveException(ErrorMessages.AlreadyConfirmed);
            }
            await _unitOfWork.Event.DeleteAsync(cancellation, id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<EventResponseModel> GetConfirmedEventByIdAsync(
          int id,
          CancellationToken cancellationToken)
        {
            Event @event = await this._unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            if (@event == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
           if(!@event.IsConfirmed)
            {
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            }
            return @event.Adapt<EventResponseModel>();
        }

        public async Task<EventResponseModel> GetUserEventAsync(
          int id,
          CancellationToken cancellationToken,
          string userId)
        {
            Event @event = await this._unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            if (@event == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if (@event.UserId != userId)
                throw new InvalidReferenceException(ErrorMessages.UserDoesNotOwnEvent);
            EventResponseModel userEventAsync = @event.Adapt<EventResponseModel>();
            return userEventAsync;
        }

        public async Task<EventResponseModel> GetUnconfirmedEventForAdminAsync(
          int id,
          CancellationToken cancellationToken)
        {
            Event @event = await _unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            if (@event == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if(@event.IsConfirmed)
            {
                throw new EventIsAlreadyActiveException(ErrorMessages.AlreadyConfirmed);
            }
            return @event.Adapt<EventResponseModel>();
        }
        public async Task UpdateEventAsync(
          EventUpdateRequestModel @event,
          int id,
          string userId,
          CancellationToken cancellationToken)
        {
            Event entity = await _unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            if (entity == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if (entity.UserId != userId)
                throw new InvalidReferenceException(ErrorMessages.UserDoesNotOwnEvent);
            if (entity.ConfirmedAt.AddDays(entity.DaysForUpdate) < DateTime.Now && entity.IsConfirmed)
                throw new UpdateDateExceededException(ErrorMessages.UpdateDateExceeded);
            entity.Title = @event.Title;
            entity.Description = @event.Description;
            entity.ImageUrl = @event.ImageUrl;
            entity.UpdatedAt = DateTime.Now;
            _unitOfWork.Event.Update(cancellationToken, entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task BookTicket(int eventId, string UserId, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.BookedTicket.Exists(cancellationToken, i => i.UserId == UserId && i.EventId == eventId))
                throw new TicketAlreadyBookedException(ErrorMessages.TicketAlreadyBooked);
            BookedTicketRequestModel entity = new BookedTicketRequestModel();
            Event @event = await _unitOfWork.Event.GetByIdAsync(cancellationToken, eventId);
            if (@event == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if (!@event.IsConfirmed)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if (@event.StartDate < DateTime.Now)
                throw new OutOfTIcketsException(ErrorMessages.AlreadyStarted);
            if (@event.NumberOfTickets == 0)
                throw new OutOfTIcketsException(ErrorMessages.OutOfTickets);
            entity.EventId = eventId;
            entity.UserId = UserId;
            entity.BookedTill = DateTime.Now.AddMinutes(@event.BookTimeInMinutes);
            --@event.NumberOfTickets;
            await _bookedTicketsService.CreateAsync(entity, cancellationToken);
            _unitOfWork.Event.Update(cancellationToken, @event);
            await _unitOfWork.SaveAsync();
        }

        public async Task BuyTicket(int id, string userId, CancellationToken cancellationToken)
        {
            Event entity = await _unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            BookedTicket book = await _unitOfWork.BookedTicket.GetByUserAndEventIdAsync(userId, id, cancellationToken);
            if (entity == null)
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            if (book != null)
            {
                await _bookedTicketsService.DeleteAsync(book.Id, cancellationToken);
            }
            else
            {
                if (!entity.IsConfirmed)
                    throw new EventNotFoundException(ErrorMessages.EventNotFound);
                if ((entity.StartDate < DateTime.Now))
                    throw new EventNotFoundException(ErrorMessages.AlreadyStarted);
                if (entity.NumberOfTickets == 0)
                    throw new OutOfTIcketsException(ErrorMessages.OutOfTickets);
                --entity.NumberOfTickets;
                _unitOfWork.Event.Update(cancellationToken, entity);
                await _unitOfWork.SaveAsync();
            }
        }
        public async Task ConfirmEvent(int id, int bookTimeInMinutes,int daysForUpdate, CancellationToken cancellationToken=default)
        {
            var @event = await _unitOfWork.Event.GetByIdAsync(cancellationToken, id);
            if(@event is null)
            {
                throw new EventNotFoundException(ErrorMessages.EventNotFound);
            }
            if(@event.IsConfirmed)
            {
                throw new EventIsAlreadyActiveException(ErrorMessages.AlreadyConfirmed);
            }
            @event.BookTimeInMinutes = bookTimeInMinutes;
            @event.DaysForUpdate= daysForUpdate;
            @event.IsConfirmed = true;
            @event.ConfirmedAt = DateTime.Now;
            _unitOfWork.Event.Update(cancellationToken,@event);
            await _unitOfWork.SaveAsync();
        }
        public async Task MoveEventsToArchive(CancellationToken cancellationToken)
        {
            List<Event> finishedEvents = await _unitOfWork.Event.GetAllFinishedEvents(cancellationToken);
            List<ArchivedEventRequestModel> adapted = finishedEvents.Adapt<List<ArchivedEventRequestModel>>();
            List<ArchivedEvent> entities = adapted.Adapt<List<ArchivedEvent>>();
            await _unitOfWork.ArchivedEvent.AddRange(entities, cancellationToken);
            _unitOfWork.Event.RemoveRange(finishedEvents);
            await _unitOfWork.SaveAsync();
        }
    }
}
