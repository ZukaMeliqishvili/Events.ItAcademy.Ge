using Application.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using System.Net;

namespace Events.It.Academy.Ge.Api.Infrastructure
{
    public class ApiError : ProblemDetails
    {
        public const string UnhandlerErrorCode = "UnhandledError";
        private readonly HttpContext _httpContext;
        private readonly Exception _exception;


        public string Code { get; set; }

        public string TraceId
        {
            get
            {
                if (Extensions.TryGetValue("TraceId", out var traceId))
                {
                    return (string)traceId;
                }

                return "";
            }

            set => Extensions["TraceId"] = value;
        }

        public ApiError(HttpContext httpContext, Exception exception)
        {
            _httpContext = httpContext;
            _exception = exception;

            TraceId = httpContext.TraceIdentifier;

            //default
            Code = UnhandlerErrorCode;
            Status = (int)HttpStatusCode.InternalServerError;
            Title = exception.Message;
            Instance = httpContext.Request.Path;

            HandleException((dynamic)exception);
        }

        private void HandleException(EventNotFoundException exception)
        {
            this.Status = new int?(404);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            this.Title = exception.Message;
        }

        private void HandleException(UserNotFoundException exception)
        {
            this.Status = new int?(404);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            this.Title = exception.Message;
        }

        private void HandleException(EventIsAlreadyActiveException exception)
        {
            this.Status = new int?(403);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            this.Title = exception.Message;
        }

        private void HandleException(InvalidUserCredentialsException exception)
        {
            this.Status = new int?(400);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            this.Title = exception.Message;
        }

        private void HandleException(InvalidReferenceException exception)
        {
            this.Status = new int?(400);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            this.Title = exception.Message;
        }

        private void HandleException(InvalidRequestException exception)
        {
            this.Status = new int?(400);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            this.Title = exception.Message;
        }

        private void HandleException(OutOfTIcketsException exception)
        {
            this.Status = new int?(403);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            this.Title = exception.Message;
        }

        private void HandleException(TicketAlreadyBookedException exception)
        {
            this.Status = new int?(403);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            this.Title = exception.Message;
        }

        private void HandleException(UpdateDateExceededException exception)
        {
            this.Status = new int?(406);
            this.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6";
            this.Title = exception.Message;
        }

        private void HandleException(Exception exception)
        {

        }
    }
}
