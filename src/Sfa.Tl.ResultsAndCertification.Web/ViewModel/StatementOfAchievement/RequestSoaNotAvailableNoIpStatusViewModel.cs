using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestSoaNotAvailableNoIpStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaNotAvailableNoIpStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaNotAvailableNoIpStatusViewModel : RequestSoaBaseViewModel
    {
        public RequestSoaNotAvailableNoIpStatusViewModel()
        {
            UlnLabel = RequestSoaNotAvailableNoIpStatusContent.Title_Uln_Text;
            LearnerNameLabel = RequestSoaNotAvailableNoIpStatusContent.Title_Name_Text;
            DateofBirthLabel = RequestSoaNotAvailableNoIpStatusContent.Title_DateofBirth_Text;
            ProviderNameLabel = RequestSoaNotAvailableNoIpStatusContent.Title_Provider_Text;
            TlevelTitleLabel = RequestSoaNotAvailableNoIpStatusContent.Title_TLevel_Text;
        }

        public bool IsIndustryPlacementAdded { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Request_Statement_Of_Achievement, RouteName = RouteConstants.RequestStatementOfAchievement },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.RequestSoaUniqueLearnerNumber },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Statement_Of_Achievement_Not_Available }
                    }
                };
            }
        }
    }
}
