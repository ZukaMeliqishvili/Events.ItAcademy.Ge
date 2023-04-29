using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application._Event.Models.Request
{
    public class EventRequestModel
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfTickets { get; set; }

        public double TicketPrice { get; set; }

        public string ImageUrl { get; set; }
    }
}
