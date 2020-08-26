using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class DateofBirthViewModel
    {
        public string Day { get; set; }

        public string Month { get; set; }
        
        public string Year { get; set; }

        public bool IsChangeMode { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.AddRegistrationLearnersName };
    }
}
