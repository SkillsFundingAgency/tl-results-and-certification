using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class RommsDownloadViewModel
    {
        public DownloadLinkViewModel RommsDownloadLinkViewModel { get; set; }
        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new() { DisplayName = BreadcrumbContent.ResultReviewsAndAppeals, RouteName = RouteConstants.ResultReviewsAndAppeals },
                        new() { DisplayName = BreadcrumbContent.Download_Romms_Data }
                    }
                };
            }
        }
    }
}
