using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;


namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class UpdateEnglishAndMathsQuestionViewModel : EnglishAndMathsQuestionViewModel
    {
        public int ProfileId { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.LearnerRecordDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}
