﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class SearchAssessmentsViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SearchAssessments), ErrorMessageResourceName = "Uln_Required_Validation_Message")]
        [RegularExpression(Constants.UlnValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.SearchAssessments), ErrorMessageResourceName = "Uln_Not_Valid_Validation_Message")]
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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Assessments }
                    }
                };
            }
        }
    }
}
