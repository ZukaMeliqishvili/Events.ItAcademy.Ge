using Application._BookedTicket;
using Application._Event;
using Application.User;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Persistance.Context;
using Persistance.Dbinitialize;

namespace Events.ItAcademy.Ge.Mvc.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<IEmailSender,EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookedTicketService, BookedTicketService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }
    }
}
