using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

using AddCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AddAssessmentEntryViewModel : AssessmentBaseViewModel
    {
        public AddAssessmentEntryViewModel()
        {
            UlnLabel = AddCoreAssessmentEntryContent.Title_Uln_Text;
            LearnerNameLabel = AddCoreAssessmentEntryContent.Title_Name_Text;
            DateofBirthLabel = AddCoreAssessmentEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = AddCoreAssessmentEntryContent.Title_Provider_Text;
            TlevelTitleLabel = AddCoreAssessmentEntryContent.Title_TLevel_Text;
        }

        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }

        [RequiredWithMessage(Property = nameof(AssessmentSeriesName), ErrorResourceType = typeof(AddCoreAssessmentEntryContent), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
        public bool? IsOpted { get; set; }

        public ComponentType ComponentType { get; set; }

        public override BackLinkModel BackLink {
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
