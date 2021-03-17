using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class EnglishAndMathsQuestionViewModel
    {
        [Required]
        public EnglishAndMathsStatus? EnglishAndMathsStatus { get; set; }
        public string LearnerName { get; set; }
        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.EnterUniqueLearnerNumber };
    }
}