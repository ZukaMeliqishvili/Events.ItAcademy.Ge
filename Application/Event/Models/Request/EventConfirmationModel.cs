using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application._Event.Models.Request
{
    public class EventConfirmationModel
    {
        [Range(1,60)]
        public int BookTimeInMinutes { get; set; }
        [Range(1,1000)]
        public int DaysForUpdate { get; set; }
    }
}
