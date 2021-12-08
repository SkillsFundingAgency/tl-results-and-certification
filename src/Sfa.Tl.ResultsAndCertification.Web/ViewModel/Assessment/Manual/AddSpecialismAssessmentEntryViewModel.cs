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

        public string SpecialismsId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }
        public List<SpecialismViewModel> SpecialismDetails { get; set; }

        [RequiredWithMessage(Property = nameof(AssessmentSeriesName), ErrorResourceType = typeof(AddSpecialismAssessmentEntryContent), ErrorResourceName = "Select_Option_To_Add_Validation_Text")]
        public bool? IsOpted { get; set; }

        public List<string> SpecialismIds => !string.IsNullOrWhiteSpace(SpecialismsId) ? SpecialismsId.Split(Constants.PipeSeperator).ToList() : new List<string>();
        public bool IsValidToAdd => SpecialismIds.Any() && IsValidSpecialismCombination;
        
        public string SpecialismDisplayName => SpecialismDetails != null && SpecialismDetails.Any() ? string.Join(Constants.AndSeperator, SpecialismDetails.Where(x => SpecialismIds.Contains(x.Id.ToString(), StringComparer.InvariantCultureIgnoreCase)).OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : null;
        private bool IsValidSpecialismCombination
        {
            get
            {
                if (SpecialismDetails == null) return false;
                                
                if(SpecialismIds.Count == 1)
                {
                    var validSpecialism = SpecialismDetails.FirstOrDefault(s => SpecialismIds[0].Equals(s.Id.ToString(), StringComparison.InvariantCultureIgnoreCase));

                    if (validSpecialism == null) return false;

                    return !validSpecialism.IsCouplet || (validSpecialism.IsCouplet && validSpecialism.IsResit);
                }
                else
                {
                    var validSpecialisms = SpecialismDetails.Where(s => SpecialismIds.Contains(s.Id.ToString(), StringComparer.InvariantCultureIgnoreCase));

                    if (!validSpecialisms.Any() || validSpecialisms.Count() != SpecialismIds.Count()) return false;

                    var coupletSpecialismCodes = validSpecialisms.SelectMany(v => v.TlSpecialismCombinations.Select(c => c.Value)).ToList();

                    var requestedSpecialismCodes = validSpecialisms.Select(s => s.LarId);

                    var hasValidCoupletSpecialismCodes = coupletSpecialismCodes.Any(cs => cs.Split(Constants.PipeSeperator).Except(requestedSpecialismCodes, StringComparer.InvariantCultureIgnoreCase).Count() == 0);

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
