using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaUniqueLearnerNumberViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.RequestSoaUniqueLearnerNumber), ErrorMessageResourceName = "Uln_Required_Validation_Message")]
        [RegularExpression(@"^\d{10}$", ErrorMessageResourceType = typeof(ErrorResource.RequestSoaUniqueLearnerNumber), ErrorMessageResourceName = "Uln_Not_Valid_Validation_Message")]
        public string SearchUln { get; set; }

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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner }
                    }
                };
            }
        }
    }
}
