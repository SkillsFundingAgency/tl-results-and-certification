﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class EnglishAndMathsLrsQuestionViewModel
    {
        // TODO: Delete this. 
        public EnglishAndMathsLrsStatus? EnglishAndMathsLrsStatus { get; set; }
        public string LearnerName { get; set; }
        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.EnterUniqueLearnerNumber };
    }
}