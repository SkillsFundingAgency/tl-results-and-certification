﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class UlnAssessmentsNotFoundViewModel : UlnNotFoundViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.SearchAssessments };
    }
}
