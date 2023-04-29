using Microsoft.AspNetCore.Identity.UI.Services;

namespace Events.ItAcademy.Ge.Mvc
{
    public class EmailSender:IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
