using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.Linq;
using AddSpecialismAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddSpecialismAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class RemoveSpecialismAssessmentEntryViewModel : AssessmentBaseViewModel
    {
        public RemoveSpecialismAssessmentEntryViewModel()
        {
            UlnLabel = AddSpecialismAssessmentEntryContent.Title_Uln_Text;
            LearnerNameLabel = AddSpecialismAssessmentEntryContent.Title_Name_Text;
            DateofBirthLabel = AddSpecialismAssessmentEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = AddSpecialismAssessmentEntryContent.Title_Provider_Text;
            TlevelTitleLabel = AddSpecialismAssessmentEntryContent.Title_TLevel_Text;
        }

        public int? SpecialismId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }
        public List<SpecialismViewModel> SpecialismDetails { get; set; }

        [RequiredWithMessage(Property = nameof(AssessmentSeriesName), ErrorResourceType = typeof(AddSpecialismAssessmentEntryContent), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
        public bool? IsOpted { get; set; }

        public bool IsResitForSpecialism => SpecialismDetails != null && SpecialismDetails.Any() && SpecialismDetails.SelectMany(sa => sa.Assessments).Any(a => a.SeriesId != AssessmentSeriesId);
        public bool DisplayMultipleSpecialismsCombined => SpecialismDetails?.Count > 1 && !IsResitForSpecialism;
        public string SpecialismDisplayName => DisplayMultipleSpecialismsCombined
                                               ? string.Join(Constants.AndSeperator, SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})"))
                                               : SpecialismDetails?.Where(x => x.Id == SpecialismId)?.Select(x => $"{x.Name} ({x.LarId})")?.FirstOrDefault();

        public bool IsValidSpecialismToRemove => DisplayMultipleSpecialismsCombined ? true : SpecialismDetails != null && SpecialismDetails.Any(x => x.Id == SpecialismId);

        public string SuccessBannerMessage { get { return string.Format(AddSpecialismAssessmentEntryContent.Banner_Message, AssessmentSeriesName, SpecialismDisplayName); } }

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
