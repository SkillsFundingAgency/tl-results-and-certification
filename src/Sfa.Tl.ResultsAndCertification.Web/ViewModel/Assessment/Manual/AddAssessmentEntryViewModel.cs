using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.Linq;
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
        public string PathwayDisplayName { get; set; }

        public List<SpecialismViewModel> SpecialismDetails { get; set; }

        public int? SpecialismId { get; set; }
        public bool IsResitForSpecialism => SpecialismDetails != null && SpecialismDetails.Any() && SpecialismDetails.SelectMany(sa => sa.Assessments).Any(a => a.SeriesId != AssessmentSeriesId);
        public bool DisplayMultipleSpecialismsCombined => SpecialismDetails?.Count > 1 && !IsResitForSpecialism;
        public string SpecialismDisplayName => DisplayMultipleSpecialismsCombined ? string.Join(Constants.AndSeperator, SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : SpecialismDetails?.Where(x => x.Id == SpecialismId)?.Select(x => $"{x.Name} ({x.LarId})")?.FirstOrDefault();

        public string SuccessBannerMessage { get { return string.Format(AddCoreAssessmentEntryContent.Banner_Message, AssessmentSeriesName, PathwayDisplayName); } }

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
