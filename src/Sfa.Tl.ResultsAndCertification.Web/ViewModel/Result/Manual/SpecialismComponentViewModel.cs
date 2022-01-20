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
    }
}