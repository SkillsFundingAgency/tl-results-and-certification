using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class PostalAddressMissingViewModel
    {
        public BackLinkModel BackLink
        {
            get { return new BackLinkModel { RouteName = RouteConstants.Home }; }
        }
    }
}
