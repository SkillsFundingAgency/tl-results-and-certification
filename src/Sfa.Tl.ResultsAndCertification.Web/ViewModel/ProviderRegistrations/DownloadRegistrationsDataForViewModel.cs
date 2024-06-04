using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations
{
    public class DownloadRegistrationsDataForViewModel
    {
        public string PageTitle { get; set; }

        public string PageHeader { get; set; }

        public string DownloadLinkText { get; set; }

        public DownloadLinkViewModel DownloadLink { get; set; }

        public BreadcrumbModel Breadcrumb { get; set; }
    }
}