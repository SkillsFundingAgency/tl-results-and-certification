using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation
{
    public class AdminDownloadResultsRommsByAwardingOrganisationViewModel
    {
        public long AwardingOrganisationUkprn { get; set; }

        public string AwardingOrganisationDisplayName { get; set; }

        public DownloadLinkViewModel CoreResultsDownloadLinkViewModel { get; set; }
        public DownloadLinkViewModel SpecialismResultsDownloadLinkViewModel { get; set; }
        public DownloadLinkViewModel RommsDownloadLinkViewModel { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminSelectAwardingOrganisation
        };
    }
}
