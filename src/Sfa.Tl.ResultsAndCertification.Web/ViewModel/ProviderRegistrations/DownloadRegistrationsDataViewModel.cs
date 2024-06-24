using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations
{
    public class DownloadRegistrationsDataViewModel
    {
        public IList<AvailableStartYearViewModel> AvailableStartYearViewModels { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new()
                {
                    DisplayName = BreadcrumbContent.Home,
                    RouteName = RouteConstants.Home
                },
                new()
                {
                    DisplayName = BreadcrumbContent.Download_Registrations_Data
                }
            }
        };
    }
}