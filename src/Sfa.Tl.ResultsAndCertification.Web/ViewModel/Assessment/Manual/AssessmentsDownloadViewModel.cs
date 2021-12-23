using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentsDownloadViewModel
    {
        public DownloadLinkViewModel CoreAssesmentsDownloadLinkViewModel { get; set; }
        public DownloadLinkViewModel SpecialismAsssmentsDownloadLinkViewModel { get; set; }

        public int CoreComponentType => (int)ComponentType.Core;
        public int SpecialismComponentType => (int)ComponentType.Specialism;

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard }
                    }
                };
            }
        }
    }
}
