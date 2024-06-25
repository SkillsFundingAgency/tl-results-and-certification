using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RegistrationsDownloadViewModel
    {
        public DownloadLinkViewModel RegistrationsDownloadLinkViewModel { get; set; }

        public DownloadLinkViewModel PendingWithdrawalsDownloadLinkViewModel { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new() { DisplayName = BreadcrumbContent.Registration_Dashboard, RouteName = RouteConstants.RegistrationDashboard },
                        new() { DisplayName = BreadcrumbContent.Download_Registrations_Data }
                    }
                };
            }
        }
    }
}
