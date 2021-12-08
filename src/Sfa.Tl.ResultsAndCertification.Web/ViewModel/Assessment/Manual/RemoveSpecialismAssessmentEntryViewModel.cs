using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
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

        public string SpecialismLarId { get; set; }
        public List<string> SpecialismLarIds => !string.IsNullOrWhiteSpace(SpecialismLarId) ? SpecialismLarId.Split(Constants.PipeSeperator).ToList() : new List<string>();
        
        public List<SpecialismViewModel> SpecialismDetails { get; set; }
        public string SpecialismDisplayName => SpecialismDetails != null && SpecialismDetails.Any() ? string.Join(Constants.AndSeperator, SpecialismDetails.Where(x => SpecialismLarIds.Contains(x.LarId, StringComparer.InvariantCultureIgnoreCase)).OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : null;
        
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
