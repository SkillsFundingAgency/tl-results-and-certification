using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults
{
    public class AdminDownloadLearnerResultsByProviderViewModel
    {
        public long ProviderUkprn { get; set; }

        public string ProviderName { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminDownloadLearnerResultsFindProvider
        };
    }
}
