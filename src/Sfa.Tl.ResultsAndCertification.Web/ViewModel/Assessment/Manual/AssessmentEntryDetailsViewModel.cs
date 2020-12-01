using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentEntryDetailsViewModel
    {
        public int ProfileId { get; set; }
        
        public int AssessmentId { get; set; }

        public string AssessmentSeriesName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.RemoveCoreAssessmentEntry), ErrorMessageResourceName = "Select_RemoveCoreAssessment_Validation_Message")]
        public bool? CanRemoveAssessmentEntry { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AssessmentDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
