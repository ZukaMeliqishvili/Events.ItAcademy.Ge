using Application._BookedTicket;
using Application._BookedTicket.Models.Request;
using Application._Event;
using Application._Event.Models.Request;
using Application.CustomExceptions;
using Application.Localization;
using Domain._Event;
using Domain.BoockedTicket;
using Mapster;
using System.Linq.Expressions;

namespace Events.ItAcademy.Tests.EventTest
{
    public class Events
    {
        //private EventRequestModel _eventRequestModel;
        private readonly string _userId = new Guid().ToString();
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Empty };
        private readonly EventService _eventService;
        private readonly CancellationToken cancellationToken = new CancellationToken();
        private readonly Mock<IBookedTicketService> _bookedTicketService;
        public Events()
        {
            var _bookService = new BookedTicketService(_unitOfWork.Object);

            _eventService = new EventService(_unitOfWork.Object, _bookService);
        }

        //eventCreate
        [Fact]
        public async Task WhenEventIsCreatedShouldNotThrowException()
        {

            _unitOfWork.Setup(x => x.Event.CreateAsync(cancellationToken, It.IsAny<Event>()));

            var task = async () => await _eventService.CreateAsync(GetEventRequestModel(), It.IsAny<string>(), cancellationToken);
            var test = await Record.ExceptionAsync(task);


            Assert.Null(test);
        }
        [Fact]
        public async Task WhenRequestModelIsNullShouldThrowArgumentNullException()
        {

            _unitOfWork.Setup(x => x.Event.CreateAsync(cancellationToken, It.IsAny<Event>()));

            var task = async () => await _eventService.CreateAsync(null, It.IsAny<string>(), cancellationToken);


            await Assert.ThrowsAsync<ArgumentNullException>(task);
        }
        [Fact]
        public async Task WhenStartDateIsHigherThanEndDateShouldThrowInvalidRequestException()
        {

            _unitOfWork.Setup(x => x.Event.CreateAsync(cancellationToken, It.IsAny<Event>()));
            var requestModel = GetEventRequestModel();
            requestModel.StartDate = DateTime.Now;
            requestModel.EndDate = DateTime.Now.AddHours(-1);
            var task = async () => await _eventService.CreateAsync(requestModel, It.IsAny<string>(), cancellationToken);
            await Assert.ThrowsAsync<InvalidRequestException>(task);
        }
        //eventUpdate
        [Fact]
        public async Task WhenEventIsUpdatedShoulNotThrowExeption()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, GetEventDomain()));
            var task = async () => await _eventService.UpdateEventAsync(GetUpdateModel(), 1, _userId, cancellationToken);
            var test = await Record.ExceptionAsync(task);
            Assert.Null(test);
        }
        [Fact]
        public async Task WhenEntityByIdNotFoundShoudThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, GetEventDomain()));
            var task = async () => await _eventService.UpdateEventAsync(GetUpdateModel(), 3, _userId, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventDoesNotBelongToUserShoudThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, GetEventDomain()));
            var task = async () => await _eventService.UpdateEventAsync(GetUpdateModel(), 1, "blablalba", cancellationToken);
            await Assert.ThrowsAsync<InvalidReferenceException>(task);
        }
        [Fact]
        public async Task WhenUpdateDateIsExceededShoudThrowException()
        {
            var entity = GetEventDomain();
            entity.ConfirmedAt = DateTime.Now.AddDays(-5);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(entity);
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, GetEventDomain()));
            var task = async () => await _eventService.UpdateEventAsync(GetUpdateModel(), 1, _userId, cancellationToken);
            await Assert.ThrowsAsync<UpdateDateExceededException>(task);
        }
        //Delete
        [Fact]
        public async Task WhenEventIsDeletedShouldNotThrowException()
        {
            var entity = GetEventDomain();
            entity.IsConfirmed = false;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(entity);
            _unitOfWork.Setup(x => x.Event.DeleteAsync(cancellationToken, 1));
            var task = async () => await _eventService.Delete(cancellationToken, 1);
            var test = await Record.ExceptionAsync(task);
            Assert.Null(test);
        }
        [Fact]
        public async Task WhenEventNotFoundShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, It.IsAny<int>())).ReturnsAsync(It.IsAny<Event>());
            _unitOfWork.Setup(x => x.Event.DeleteAsync(cancellationToken, It.IsAny<int>()));
            var task = async () => await _eventService.Delete(cancellationToken, It.IsAny<int>());
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventIsConfirmedShoudThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            _unitOfWork.Setup(x => x.Event.DeleteAsync(cancellationToken, 1));
            var task = async () => await _eventService.Delete(cancellationToken, 1);
            await Assert.ThrowsAsync<EventIsAlreadyActiveException>(task);
        }

        //GetConfirmedEventById
        [Fact]
        public async Task WhenConfirmedEventIsReturnedShoudNotThrowException()
        {
            var @event = GetEventDomain();
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            var model = await _eventService.GetConfirmedEventByIdAsync(1, cancellationToken);
            Assert.Equal(@event.Id, model.Id);
        }
        [Fact]
        public async Task WhenConfirmedEventNotFoundShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            var task = async () => await _eventService.GetConfirmedEventByIdAsync(2, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventIsNotConfirmedShoudThrowException()
        {
            var entity = GetEventDomain();
            entity.IsConfirmed = false;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(entity);
            var task = async () => await _eventService.GetConfirmedEventByIdAsync(1, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        //getUserEvent
        [Fact]
        public async Task WhenUserEventIsReturnedShouldNotThrowException()
        {
            var @event = GetEventDomain();
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            var model = await _eventService.GetUserEventAsync(1, cancellationToken, _userId);
            Assert.Equal(@event.Id, model.Id);
        }
        [Fact]
        public async Task WhenUserEventNotFoundShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            var task = async () => await _eventService.GetUserEventAsync(2, cancellationToken, _userId);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventDoesNotBelongToUserShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            var task = async () => await _eventService.GetUserEventAsync(1, cancellationToken, "blabla");
            await Assert.ThrowsAsync<InvalidReferenceException>(task);
        }
        //GetUnconfirmedEventForAdmin
        [Fact]
        public async Task WhenUnconfirmedEventIsReturnedShouldNotThrowException()
        {
            var entity = GetEventDomain();
            entity.IsConfirmed = false;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(entity);
            var model = await _eventService.GetUnconfirmedEventForAdminAsync(1, cancellationToken);
            Assert.Equal(entity.Id, model.Id);
        }
        [Fact]
        public async Task WhenEventIsConfimedShoudThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            var task = async () => await _eventService.GetUnconfirmedEventForAdminAsync(1, cancellationToken);
            await Assert.ThrowsAsync<EventIsAlreadyActiveException>(task);
        }
        //BookTicket
        [Fact]
        public async Task WhenUserDoesNotHaveBookInTableShouldNotThrowException()
        {
            var @event = GetEventDomain();
            int ticketsCount = @event.NumberOfTickets;
            var entity = new BookedTicketRequestModel
            {

                UserId = _userId,
                EventId = @event.Id,
                BookedTill = DateTime.Now.AddMinutes(@event.BookTimeInMinutes)
            };
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()));
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.CreateAsync(cancellationToken, entity.Adapt<BookedTicket>()));
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, @event));
            await _eventService.BookTicket(@event.Id, _userId, cancellationToken);
            Assert.Equal(++@event.NumberOfTickets, ticketsCount);
        }
        [Fact]
        public async Task WhenUserAlreadyBookedTicketShouldThrowException()
        {
            var @event = GetEventDomain();
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()))
                .ReturnsAsync(true);
            var task = async () => await _eventService.BookTicket(@event.Id, _userId, cancellationToken);
            await Assert.ThrowsAsync<TicketAlreadyBookedException>(task);
        }
        [Fact]
        public async Task WhenWhileBookingEventDoesNotExsitsShouldThrowException()
        {
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()))
                .ReturnsAsync(false);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, It.IsAny<int>()));
            var task = async () => await _eventService.BookTicket(2, _userId, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenWhileBookingEventIsNotConfirmedShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.IsConfirmed = false;
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()))
                .ReturnsAsync(false);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            var task = async () => await _eventService.BookTicket(1, _userId, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenWhileBookingEventAlreadyStartedShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.StartDate = DateTime.Now.AddHours(-1);
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()))
                .ReturnsAsync(false);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            var task = async () => await _eventService.BookTicket(1, _userId, cancellationToken);
            var record = await Record.ExceptionAsync(task);
            Assert.Matches(ErrorMessages.AlreadyStarted, record.Message);
        }
        [Fact]
        public async Task WhenWhileBookingOutOfTicketsShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.NumberOfTickets = 0;
            _unitOfWork.Setup(x => x.BookedTicket.Exists(cancellationToken, It.IsAny<Expression<Func<BookedTicket, bool>>>()))
                .ReturnsAsync(false);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            var task = async () => await _eventService.BookTicket(1, _userId, cancellationToken);
            var record = await Record.ExceptionAsync(task);
            Assert.Matches(ErrorMessages.OutOfTickets, record.Message);
        }
        [Fact]
        public async Task WhenTicketWasNotBookedShouldDecraseTicketCount()
        {
            var @event = GetEventDomain();
            int tickets = @event.NumberOfTickets;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(It.IsAny<string>(), It.IsAny<int>(), cancellationToken));
            _unitOfWork.Setup(x => x.Event.Update(cancellationToken, @event));
            await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            Assert.Equal(--tickets, @event.NumberOfTickets);
        }
        [Fact]
        public async Task WhenTicketWasBookedShouldNotDecraseTicketCount()
        {
            var @event = GetEventDomain();
            int tickets = @event.NumberOfTickets;
            var book = new BookedTicket
            {
                Id = 1,
                EventId = @event.Id,
                UserId = _userId,
                BookedTill = DateTime.Now
            };
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(_userId, @event.Id, cancellationToken))
                .ReturnsAsync(book);
            _unitOfWork.Setup(x => x.BookedTicket.DeleteAsync(cancellationToken, book.Id));
            await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            Assert.Equal(tickets, @event.NumberOfTickets);
        }
        [Fact]
        public async Task WhenWhileBuyingEventNotFoundShouldThrowException()
        {
            var @event = GetEventDomain();
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, It.IsAny<int>()));
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(It.IsAny<string>(), It.IsAny<int>(), cancellationToken));
            var task = async () => await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventIsNotConfirmedShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.IsConfirmed = false;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(_userId, @event.Id, cancellationToken))
                .ReturnsAsync(It.IsAny<BookedTicket>());
            var task = async () => await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            var record = await Record.ExceptionAsync(task);
            Assert.Matches(ErrorMessages.EventNotFound, record.Message);
        }
        [Fact]
        public async Task WhenEventIsAlreadyStartedShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.StartDate = DateTime.Now.AddHours(-1);
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(_userId, @event.Id, cancellationToken))
                .ReturnsAsync(It.IsAny<BookedTicket>());
            var task = async () => await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            var record = await Record.ExceptionAsync(task);
            Assert.Matches(ErrorMessages.AlreadyStarted, record.Message);
        }
        [Fact]
        public async Task WhenOutOfTicketsShouldThrowException()
        {
            var @event = GetEventDomain();
            @event.NumberOfTickets = 0;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            _unitOfWork.Setup(x => x.BookedTicket.GetByUserAndEventIdAsync(_userId, @event.Id, cancellationToken))
                .ReturnsAsync(It.IsAny<BookedTicket>());
            var task = async () => await _eventService.BuyTicket(@event.Id, _userId, cancellationToken);
            await Assert.ThrowsAsync<OutOfTIcketsException>(task);
        }
        //confirm event
        [Fact]

        public async Task WhenEverythingIsAllrightIsConfirmedMustBeTrue()
        {
            var @event = GetEventDomain();
            @event.IsConfirmed = false;
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(@event);
            int daysforupdate = 10;
            int booktimeinminutes = 5;
            await _eventService.ConfirmEvent(@event.Id, booktimeinminutes, daysforupdate);
            Assert.Equal(booktimeinminutes, @event.BookTimeInMinutes);
            Assert.Equal(daysforupdate, @event.DaysForUpdate);
            Assert.True(@event.IsConfirmed);
        }
        [Fact]
        public async Task WhenEventIsNullWhileConfrimationShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, It.IsAny<int>()));
            int daysforupdate = 10;
            int booktimeinminutes = 5;
            var task = async () => await _eventService.ConfirmEvent(1, booktimeinminutes, daysforupdate);
            await Assert.ThrowsAsync<EventNotFoundException>(task);
        }
        [Fact]
        public async Task WhenEventIsActiveWhileConfrimationShouldThrowException()
        {
            _unitOfWork.Setup(x => x.Event.GetByIdAsync(cancellationToken, 1)).ReturnsAsync(GetEventDomain());
            int daysforupdate = 10;
            int booktimeinminutes = 5;
            var task = async () => await _eventService.ConfirmEvent(1, booktimeinminutes, daysforupdate);
            await Assert.ThrowsAsync<EventIsAlreadyActiveException>(task);
        }
        //archiver
        [Fact]
        public async Task WhenFinishedEventsAreArchivedShouldNotThrowException()
        {
            var list = GetFinishedEvents();
            _unitOfWork.Setup(x => x.Event.GetAllFinishedEvents(cancellationToken))
                .ReturnsAsync(list);
            var adapted = list.Adapt<List<ArchivedEventRequestModel>>();
            _unitOfWork.Setup(x => x.ArchivedEvent.AddRange(It.IsAny<List<ArchivedEvent>>(), cancellationToken));
            _unitOfWork.Setup(x => x.Event.RemoveRange(list));
            _unitOfWork.Setup(x => x.SaveAsync());

            var task = async () => await _eventService.MoveEventsToArchive(cancellationToken);
            var record = await Record.ExceptionAsync(task);
            Assert.Null(record);
        }

        private EventRequestModel GetEventRequestModel()
        {
            EventRequestModel eventRequest = new EventRequestModel()
            {
                Title = "Some Event",
                Description = "Come and enjoy",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3),
                NumberOfTickets = 1000,
                TicketPrice = 35,
                ImageUrl = "",
            };
            return eventRequest;
        }
        private EventUpdateRequestModel GetUpdateModel()
        {
            return new EventUpdateRequestModel
            {
                Title = "New Title",
                Description = "New Description",
                ImageUrl = ""
            };
        }
        private Event GetEventDomain()
        {
            return new Event
            {
                Id = 1,
                Title = "Some Event",
                Description = "Come and enjoy",
                StartDate = DateTime.Now.AddHours(1),
                EndDate = DateTime.Now.AddHours(3),
                NumberOfTickets = 1000,
                TicketPrice = 35,
                ImageUrl = "",
                ConfirmedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsConfirmed = true,
                DaysForUpdate = 1,
                BookTimeInMinutes = 1,
                UserId = _userId
            };
        }
        private List<Event> GetFinishedEvents()
        {
            var list = new List<Event>
            {
                new Event
                {
                         Id = 1,
                    Title = "Some Event",
                    Description = "Come and enjoy",
                    StartDate = DateTime.Now.AddHours(-3),
                    EndDate = DateTime.Now.AddHours(-1),
                    NumberOfTickets = 1000,
                    TicketPrice = 35,
                    ImageUrl = "",
                    ConfirmedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsConfirmed = true,
                    DaysForUpdate = 1,
                    BookTimeInMinutes = 1,
                    UserId = _userId
                },
                new Event
                {
                        Id = 2,
                    Title = "Some Event",
                    Description = "Come and enjoy",
                    StartDate = DateTime.Now.AddHours(-3),
                    EndDate = DateTime.Now.AddHours(-1),
                    NumberOfTickets = 1000,
                    TicketPrice = 35,
                    ImageUrl = "",
                    ConfirmedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsConfirmed = true,
                    DaysForUpdate = 1,
                    BookTimeInMinutes = 1,
                    UserId = _userId
                }
            };
            return list;
        }
    }
}