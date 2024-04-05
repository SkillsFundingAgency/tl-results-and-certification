using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminSearchChangeLogViewModel
    {
        private const int FirstPage = 1;
        private const int PageSize = 10;

        public AdminSearchChangeLogCriteriaViewModel SearchCriteriaViewModel { get; set; } = new AdminSearchChangeLogCriteriaViewModel();

        public IList<AdminSearchChangeLogDetailsViewModel> ChangeLogDetails { get; set; } = new List<AdminSearchChangeLogDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }

        public PaginationModel Pagination => new()
        {
            PagerInfo = PagerInfo,
            RouteName = RouteConstants.AdminSearchChangeLog,
            PaginationSummary = AdminSearchChangeLog.PaginationSummary_Text
        };

        public bool ContainsResults
            => !ChangeLogDetails.IsNullOrEmpty() && ChangeLogDetails.Count > 0;

        public bool ContainsMultiplePages
            => ContainsResults && Pagination?.PagerInfo?.TotalPages > 1;

        public void SetSearchKey(string searchKey)
        {
            SearchCriteriaViewModel ??= new AdminSearchChangeLogCriteriaViewModel();
            SearchCriteriaViewModel.SearchKey = searchKey;
            SearchCriteriaViewModel.PageNumber = FirstPage;
        }

        public void ClearSearchKey()
            => SetSearchKey(string.Empty);

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.AdminHome },
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Change_Log }
            }
        };

    }
}