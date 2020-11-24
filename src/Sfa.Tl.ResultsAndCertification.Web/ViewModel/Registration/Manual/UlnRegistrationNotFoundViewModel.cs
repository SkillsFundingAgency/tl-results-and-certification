using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class UlnRegistrationNotFoundViewModel : UlnNotFoundViewModel
    {        
        public bool IsChangeMode { get; set; }
        public string BackLinkRouteName { get; set; }

        public bool IsUlnRegisteredAlready
        {
            get
            {
                return IsAllowed || IsRegisteredWithOtherAo;
            }
        }

        public override BackLinkModel BackLink => new BackLinkModel { RouteName = !string.IsNullOrWhiteSpace(BackLinkRouteName) ? BackLinkRouteName : RouteConstants.AddRegistrationUln, RouteAttributes = IsChangeMode ? new Dictionary<string, string> { { Constants.IsChangeMode, "true" } } : null };
    }
}
