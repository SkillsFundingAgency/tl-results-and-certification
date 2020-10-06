namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregistrationResponse
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSelectedCoreSameAsWithdrawn { get; set; }
    }
}
