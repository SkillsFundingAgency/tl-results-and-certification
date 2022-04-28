using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpCompletionViewModel
    {
        public int ProfileId { get; set; }
        public int PathwayId { get; set; }
        public int AcademicYear { get; set; }
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IndustryPlacementQuestion), ErrorMessageResourceName = "Validation_Select_Industry_Placement_Required_Message")]
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }

        public bool IsValid => IndustryPlacementStatus != ResultsAndCertification.Common.Enum.IndustryPlacementStatus.Completed || IndustryPlacementStatus == ResultsAndCertification.Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration;
        
        public virtual BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.LearnerRecordDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}