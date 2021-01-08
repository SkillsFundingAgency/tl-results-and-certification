using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class UlnResultsNotFoundViewModel : UlnNotFoundViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.SearchResults };
    }
}
