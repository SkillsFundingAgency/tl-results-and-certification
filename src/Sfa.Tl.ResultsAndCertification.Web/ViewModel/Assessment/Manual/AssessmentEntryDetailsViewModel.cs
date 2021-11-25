using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RemoveCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentEntryDetailsViewModel : AssessmentBaseViewModel
    {
        public AssessmentEntryDetailsViewModel()
        {
            UlnLabel = RemoveCoreAssessmentEntryContent.Title_Uln_Text;
            LearnerNameLabel = RemoveCoreAssessmentEntryContent.Title_Name_Text;
            DateofBirthLabel = RemoveCoreAssessmentEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = RemoveCoreAssessmentEntryContent.Title_Provider_Text;
            TlevelTitleLabel = RemoveCoreAssessmentEntryContent.Title_TLevel_Text;
        }

        public int AssessmentId { get; set; }

        public string AssessmentSeriesName { get; set; }

        [Required(ErrorMessageResourceType = typeof(RemoveCoreAssessmentEntryContent), ErrorMessageResourceName = "Select_Option_To_Remove_Validation_Text")]
        public bool? CanRemoveAssessmentEntry { get; set; }

        public ComponentType ComponentType { get; set; }

        public override BackLinkModel BackLink
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
