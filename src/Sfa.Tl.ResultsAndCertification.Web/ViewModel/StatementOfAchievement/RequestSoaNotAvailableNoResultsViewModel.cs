using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestSoaNotAvailableNoResultsContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaNotAvailableNoResults;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaNotAvailableNoResultsViewModel : RequestSoaBaseViewModel
    {
        public RequestSoaNotAvailableNoResultsViewModel()
        {
            UlnLabel = RequestSoaNotAvailableNoResultsContent.Title_Uln_Text;
            LearnerNameLabel = RequestSoaNotAvailableNoResultsContent.Title_Name_Text;
            DateofBirthLabel = RequestSoaNotAvailableNoResultsContent.Title_DateofBirth_Text;
            ProviderNameLabel = RequestSoaNotAvailableNoResultsContent.Title_Provider_Text;
            TlevelTitleLabel = RequestSoaNotAvailableNoResultsContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }

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
