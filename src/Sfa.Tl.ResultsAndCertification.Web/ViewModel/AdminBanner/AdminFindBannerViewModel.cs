using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner
{
    public class AdminFindBannerViewModel
    {
        public AdminFindBannerCriteriaViewModel SearchCriteriaViewModel { get; set; } = new AdminFindBannerCriteriaViewModel();

        public IList<AdminFindBannerDetailsViewModel> Details { get; set; } = new List<AdminFindBannerDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }

        public PaginationModel Pagination => new()
        {
            PagerInfo = PagerInfo,
            RouteName = RouteConstants.AdminFindBanner,
            PaginationSummary = AdminFindBanner.PaginationSummary_Text
        };

        public bool ContainsResults
            => !Details.IsNullOrEmpty() && Details.Count > 0;

        public bool ContainsMultiplePages
            => ContainsResults && Pagination?.PagerInfo?.TotalPages > 1;

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.AdminHome },
                new() { DisplayName = BreadcrumbContent.Find_Banners }
            }
        };
    }
}