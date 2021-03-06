using System.ComponentModel.DataAnnotations;

namespace Parbad.Sample.EntityFrameworkCore.Models
{
    public class RefundViewModel
    {
        [Display(Name = "Tracking number")]
        public long TrackingNumber { get; set; }
    }
}
