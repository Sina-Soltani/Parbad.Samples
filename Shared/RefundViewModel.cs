using System.ComponentModel.DataAnnotations;

namespace Parbad.Sample.Shared
{
    public class RefundViewModel
    {
        [Display(Name = "Tracking number")]
        public long TrackingNumber { get; set; }
    }
}
