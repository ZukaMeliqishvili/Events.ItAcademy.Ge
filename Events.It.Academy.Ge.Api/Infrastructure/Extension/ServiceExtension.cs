using Application._BookedTicket;
using Application._Event;
using Application.User;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Persistance.Context;
using Persistance.Dbinitialize;

namespace Events.It.Academy.Ge.Api.Infrastructure.Extension
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookedTicketService, BookedTicketService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }
    }
}
