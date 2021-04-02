using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class UpdateIndustryPlacementQuestionViewModel : IndustryPlacementQuestionViewModel
    {        
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int IndustryPlacementId { get; set; }
        public bool IsLearnerRecordAdded { get; set; }

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.LearnerRecordDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}