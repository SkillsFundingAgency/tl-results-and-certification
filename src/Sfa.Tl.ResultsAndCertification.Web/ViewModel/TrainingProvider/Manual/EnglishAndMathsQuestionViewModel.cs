using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class EnglishAndMathsQuestionViewModel
    {
        public EnglishAndMathsStatus? EnglishAndMathsStatus { get; set; }

        public string LearnerName { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.EnterUniqueLearnerNumber
        };
    }
}
