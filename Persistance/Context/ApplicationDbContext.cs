using Domain._Event;
using Domain.BoockedTicket;
using Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Persistance.Context
{

        public class ApplicationDbContext : IdentityDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base((DbContextOptions)options)
            {
            }

        public DbSet<Event> Events { get; set; }

        public DbSet<ArchivedEvent> ArchivedEvents { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<BookedTicket> BookedTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            }
        }
 }

