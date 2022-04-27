using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerRecordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SearchLearnerRecord), ErrorMessageResourceName = "Uln_Required_Validation_Message")]
        [RegularExpression(Constants.UlnValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.SearchLearnerRecord), ErrorMessageResourceName = "Uln_Not_Valid_Validation_Message")]
        public string SearchUln { get; set; }

        public bool IsLearnerRegistered { get; set; }

        public bool IsLearnerRecordAdded { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Manage_Learner_Records }
                    }
                };
            }
        }
    }
}
