using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RemoveEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveSpecialismAssessmentEntries;


namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class RemoveSpecialismAssessmentEntryViewModel : AssessmentBaseViewModel
    {
        public RemoveSpecialismAssessmentEntryViewModel()
        {
            UlnLabel = RemoveEntryContent.Title_Uln_Text;
            LearnerNameLabel = RemoveEntryContent.Title_Name_Text;
            DateofBirthLabel = RemoveEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = RemoveEntryContent.Title_Provider_Text;
            TlevelTitleLabel = RemoveEntryContent.Title_TLevel_Text;
        }

        [Required(ErrorMessageResourceType = typeof(RemoveEntryContent), ErrorMessageResourceName = "Select_Option_To_Remove_Validation_Text")]
        public bool? CanRemoveAssessmentEntry { get; set; }
        public string AssessmentSeriesName { get; set; }

        public string SpecialismAssessmentIds { get; set; }
        public List<int> SpecialismAssessmentIdList => !string.IsNullOrWhiteSpace(SpecialismAssessmentIds) ? SpecialismAssessmentIds.Split(Constants.PipeSeperator)?.Select(x => x.ToInt())?.ToList() : new List<int>();
        
        public List<SpecialismViewModel> SpecialismDetails { get; set; }
        public string SpecialismDisplayName
        {
            get
            {
                if (SpecialismDetails == null || !SpecialismDetails.Any())
                    return null;

                return string.Join(Constants.AndSeperator,
                                    SpecialismDetails.Where(x => x.Assessments.Any(a => SpecialismAssessmentIdList.Contains(a.AssessmentId)))
                                    .OrderBy(x => x.Name)
                                    .Select(x => $"{x.Name} ({x.LarId})"));
            }
        }

        public string SuccessBannerMessage { get { return string.Format(RemoveEntryContent.Banner_Message, SpecialismDisplayName, AssessmentSeriesName); } }

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
