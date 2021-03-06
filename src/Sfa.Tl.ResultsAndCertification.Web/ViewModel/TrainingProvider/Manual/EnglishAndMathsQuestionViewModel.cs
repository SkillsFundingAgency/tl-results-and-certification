﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class EnglishAndMathsQuestionViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.EnglishAndMathsQuestion), ErrorMessageResourceName = "Validation_Select_Is_EnglishMaths_Achieved_Required_Message")]
        public EnglishAndMathsStatus? EnglishAndMathsStatus { get; set; }
        public string LearnerName { get; set; }
        public bool IsChangeMode { get; set; }
        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddLearnerRecordCheckAndSubmit :  RouteConstants.EnterUniqueLearnerNumber };
    }
}