using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Events.ItAcademy.Ge.Mvc.Models.EventVM
{
    public class EventResponseVM
    {
        public int Id { get; set; }

        [Required, NotNull]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required, NotNull]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfTickets { get; set; }

        public double TicketPrice { get; set; }
        public string ImageUrl { get; set; }
        [ValidateNever]
        public string UserId { get; set; }

        public bool IsConfirmed { get; set; }
        public DateTime ConfirmedAt { get; set; }
        public int DaysForUpdate { get; set; }
    }
}
