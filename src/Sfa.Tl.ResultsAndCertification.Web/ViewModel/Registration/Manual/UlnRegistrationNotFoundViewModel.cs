using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class UlnRegistrationNotFoundViewModel
    {
        public string Uln { get; set; }
        public int RegistrationProfileId { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }
        public bool IsActive { get; set; }
        public bool IsChangeMode { get; set; }
        public string BackLinkRouteName { get; set; }

        public bool IsUlnRegisteredAlready
        {
            get
            {
                return IsActive || IsRegisteredWithOtherAo;
            }
        }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = !string.IsNullOrWhiteSpace(BackLinkRouteName) ? BackLinkRouteName : RouteConstants.AddRegistrationUln, RouteAttributes = IsChangeMode ? new Dictionary<string, string> { { Constants.IsChangeMode, "true" } } : null };
    }
}
