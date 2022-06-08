﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class RegisteredLearnersViewModel
    {
        public SearchCriteriaViewModel SearchCriteria { get; set; }
        public SearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem> { new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home } }
        };
    }
}
