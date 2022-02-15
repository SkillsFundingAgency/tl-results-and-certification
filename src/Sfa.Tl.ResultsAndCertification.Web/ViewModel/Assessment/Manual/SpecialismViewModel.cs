using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class SpecialismViewModel
    {
        public string CombinedSpecialismId { get; set; }
        public int Id { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? CurrentSpecialismAssessmentSeriesId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismCombinations { get; set; }
        public IEnumerable<SpecialismAssessmentViewModel> Assessments { get; set; }

        public bool IsCouplet => TlSpecialismCombinations != null && TlSpecialismCombinations.Any();
        public bool HasCurrentAssessmentEntry => Assessments != null && CurrentSpecialismAssessmentSeriesId.HasValue && Assessments.Any(a => a.SeriesId == CurrentSpecialismAssessmentSeriesId.Value);
        public bool IsResit => Assessments != null && CurrentSpecialismAssessmentSeriesId.HasValue && Assessments.Any(a => a.SeriesId != CurrentSpecialismAssessmentSeriesId.Value);
        public bool NeedResultForPreviousAssessmentEntry => !HasCurrentAssessmentEntry && HasPreviousAssessment && !HasResultForPreviousAssessment;

        private SpecialismAssessmentViewModel PreviousAssessment => Assessments?.Where(a => a.SeriesId != CurrentSpecialismAssessmentSeriesId)?.OrderByDescending(a => a.SeriesId)?.FirstOrDefault();
        private bool HasPreviousAssessment => PreviousAssessment != null;
        private bool HasResultForPreviousAssessment => HasPreviousAssessment && PreviousAssessment.Result != null && PreviousAssessment.Result.Id > 0;        
    }
}
