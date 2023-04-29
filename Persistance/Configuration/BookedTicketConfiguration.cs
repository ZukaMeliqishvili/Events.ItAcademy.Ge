using Domain._Event;
using Domain.BoockedTicket;
using Domain.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configuration
{
    public class BookedTIcketConfiguration : IEntityTypeConfiguration<BookedTicket>
    {
        public void Configure(EntityTypeBuilder<BookedTicket> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.HasOne(x => x.User).WithMany(x => x.BookedTicket).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.Event).WithMany(x=>x.BookedTickets).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
