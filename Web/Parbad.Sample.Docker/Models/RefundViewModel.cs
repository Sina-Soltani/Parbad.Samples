using System.ComponentModel.DataAnnotations;

namespace Parbad.Sample.Docker.Models
{
    public class RefundViewModel
    {
        [Display(Name = "Tracking number")]
        public long TrackingNumber { get; set; }
    }
}
