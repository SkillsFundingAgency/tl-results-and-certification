namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Error
{
    public class ProblemWithServiceViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string TechnicalSupportEmailAddress { get; set; }
    }
}
