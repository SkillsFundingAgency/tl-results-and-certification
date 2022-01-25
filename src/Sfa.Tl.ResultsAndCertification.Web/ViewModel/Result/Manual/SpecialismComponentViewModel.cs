using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class SpecialismComponentViewModel
    {
        public SpecialismComponentViewModel()
        {
            SpecialismComponentExams = new List<ComponentExamViewModel>();
        }

        public string SpecialismComponentDisplayName { get; set; }
        public bool IsSpecialismAssessmentEntryRegistered { get { return SpecialismComponentExams.Any(x => x.AssessmentId > 0); } }
        public IList<ComponentExamViewModel> SpecialismComponentExams { get; set; }

        
        #region Properties_For_Couplets_Display
        
        public int Id { get; set; }
        public string LarId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismCombinations { get; set; }
        public bool IsCouplet => TlSpecialismCombinations != null && TlSpecialismCombinations.Any();
        public string CombinedSpecialismId { get; set; }

        #endregion 
    }
}