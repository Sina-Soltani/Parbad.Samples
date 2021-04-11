namespace Parbad.Sample.Angular.Models
{
    public class PayViewModel
    {
        public long TrackingNumber { get; set; }

        public bool GenerateTrackingNumberAutomatically { get; set; }

        public decimal Amount { get; set; }

        public string SelectedGateway { get; set; }
    }
}
