﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerRecordNotFoundViewModel : LearnerRecordNotFoundViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.SearchLearnerRecord
        };
    }
}
