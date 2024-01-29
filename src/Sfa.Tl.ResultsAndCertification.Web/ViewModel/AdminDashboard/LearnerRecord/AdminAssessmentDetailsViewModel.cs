using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminAssessmentDetailsViewModel
    {
        public int RegistrationPathwayId { get; set; }

        #region Core component

        public string PathwayDisplayName { get; set; }

        public bool IsCoreEntryEligible { get; set; }

        public bool NeedCoreResultForPreviousAssessmentEntry => !HasCurrentCoreAssessmentEntry && HasPreviousCoreAssessment && !HasResultForPreviousCoreAssessment;

        public bool HasCurrentCoreAssessmentEntry => PathwayAssessment != null;

        public string NextAvailableCoreSeries { get; set; }

        public PathwayAssessmentViewModel PathwayAssessment { get; set; }

        public PathwayAssessmentViewModel PreviousPathwayAssessment { get; set; }

        public bool HasPreviousCoreAssessment
            => PreviousPathwayAssessment != null;

        public bool HasResultForPreviousCoreAssessment
            => HasPreviousCoreAssessment && PreviousPathwayAssessment.Result != null && PreviousPathwayAssessment.Result.Id > 0;

        #endregion

        #region Occupational specialism

        public List<AdminSpecialismViewModel> SpecialismDetails { get; set; }

        public bool IsSpecialismRegistered => SpecialismDetails.Any();

        public bool IsSpecialismEntryEligible { get; set; }

        public string NextAvailableSpecialismSeries { get; set; }

        public List<AdminSpecialismViewModel> DisplaySpecialisms
        {
            get
            {
                var specialismToDisplay = new List<AdminSpecialismViewModel>();

                if (SpecialismDetails == null) return specialismToDisplay;

                foreach (var specialism in SpecialismDetails)
                {
                    if (specialism.IsCouplet && specialism.IsResit == false)
                    {
                        foreach (var spCombination in specialism.TlSpecialismCombinations)
                        {
                            var pairedSpecialismCodes = spCombination.Value.Split(Constants.PipeSeperator).Except(new List<string> { specialism.LarId }, StringComparer.InvariantCultureIgnoreCase);

                            var combinedSpecialismId = specialism.Id.ToString();
                            var combinedDisplayName = specialism.DisplayName;
                            var hasValidEntry = false;

                            foreach (var pairedSpecialismCode in pairedSpecialismCodes)
                            {
                                var validSpecialism = SpecialismDetails.FirstOrDefault(s => s.LarId.Equals(pairedSpecialismCode, StringComparison.InvariantCultureIgnoreCase));

                                if (validSpecialism != null)
                                {
                                    hasValidEntry = true;
                                    combinedSpecialismId = $"{combinedSpecialismId}{Constants.PipeSeperator}{validSpecialism.Id}";
                                    combinedDisplayName = $"{combinedDisplayName}{Constants.AndSeperator}{validSpecialism.DisplayName}";
                                }
                            }

                            var canAdd = hasValidEntry && !specialismToDisplay.Any(s => combinedSpecialismId.Split(Constants.PipeSeperator).Except(s.CombinedSpecialismId.Split(Constants.PipeSeperator), StringComparer.InvariantCultureIgnoreCase).Any());

                            if (canAdd)
                            {
                                specialismToDisplay.Add(new AdminSpecialismViewModel
                                {
                                    CombinedSpecialismId = combinedSpecialismId,
                                    DisplayName = combinedDisplayName,
                                });
                            }
                        }
                    }
                    else
                    {
                        specialismToDisplay.Add(specialism);
                    }
                }

                return specialismToDisplay;
            }
        }

        #endregion
    }
}