using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AddAssessmentEntryViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }

        [RequiredWithMessage(Property = nameof(AssessmentSeriesName), ErrorResourceType = typeof(ErrorResource.AddCoreAssessmentEntry), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
        public bool? IsOpted { get; set; }

        public ComponentType ComponentType { get; set; }
        public BackLinkModel BackLink {
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
