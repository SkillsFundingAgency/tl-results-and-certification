using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.Linq;
using AddSpecialismAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddSpecialismAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AddSpecialismAssessmentEntryViewModel : AssessmentBaseViewModel
    {
        public AddSpecialismAssessmentEntryViewModel()
        {
            UlnLabel = AddSpecialismAssessmentEntryContent.Title_Uln_Text;
            LearnerNameLabel = AddSpecialismAssessmentEntryContent.Title_Name_Text;
            DateofBirthLabel = AddSpecialismAssessmentEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = AddSpecialismAssessmentEntryContent.Title_Provider_Text;
            TlevelTitleLabel = AddSpecialismAssessmentEntryContent.Title_TLevel_Text;
        }

        public string SpecialismLarId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }
        public List<SpecialismViewModel> SpecialismDetails { get; set; }

        [RequiredWithMessage(Property = nameof(AssessmentSeriesName), ErrorResourceType = typeof(AddSpecialismAssessmentEntryContent), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
        public bool? IsOpted { get; set; }

        public List<string> SpecialismLarIds => !string.IsNullOrWhiteSpace(SpecialismLarId) ? SpecialismLarId.Split(Constants.PipeSeperator).ToList() : new List<string>();
        public bool IsValidToAdd => SpecialismLarIds.Any() ? IsValidSpecialismCombination : false;
        
        public string SpecialismDisplayName => SpecialismDetails != null && SpecialismDetails.Any() ? string.Join(Constants.AndSeperator, SpecialismDetails.Where(x => SpecialismLarIds.Contains(x.LarId, StringComparer.InvariantCultureIgnoreCase)).OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : null;
        private bool IsValidSpecialismCombination
        {
            get
            {
                if (SpecialismDetails == null) return false;
                                
                if(SpecialismLarIds.Count == 1)
                {
                    var validSpecialism = SpecialismDetails.FirstOrDefault(s => s.LarId.Equals(SpecialismLarIds[0], StringComparison.InvariantCultureIgnoreCase));

                    if (validSpecialism == null) return false;

                    return !validSpecialism.IsCouplet || (validSpecialism.IsCouplet && validSpecialism.IsResit);
                }
                else
                {
                    var validSpecialisms = SpecialismDetails.Where(s => SpecialismLarIds.Contains(s.LarId, StringComparer.InvariantCultureIgnoreCase));

                    if (!validSpecialisms.Any() || validSpecialisms.Count() != SpecialismLarIds.Count()) return false;

                    var coupletSpecialismCodes = validSpecialisms.SelectMany(v => v.TlSpecialismCombinations.Select(c => c.Value)).ToList();
                    
                    //var specialismCodes = SpecialismDetails.Select(s => s.LarId);

                    var hasValidCoupletSpecialismCodes = coupletSpecialismCodes.Any(cs => cs.Split(Constants.PipeSeperator).Except(SpecialismLarIds, StringComparer.InvariantCultureIgnoreCase).Count() == 0);

                    return validSpecialisms.All(v => v.IsCouplet && !v.IsResit) && hasValidCoupletSpecialismCodes;
                }
            }
        }

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
