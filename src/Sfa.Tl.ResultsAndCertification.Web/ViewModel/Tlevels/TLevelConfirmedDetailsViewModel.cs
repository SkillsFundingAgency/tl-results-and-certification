using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TLevelConfirmedDetailsViewModel : TlevelSummary
    {
        public TLevelConfirmedDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        public int PathwayId { get; set; }
        public bool IsValid { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ConfirmedTlevels
                };
            }
        }
    }
}
