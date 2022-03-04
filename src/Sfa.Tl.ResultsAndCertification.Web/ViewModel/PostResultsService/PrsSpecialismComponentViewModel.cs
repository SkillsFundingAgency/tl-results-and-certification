using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsSpecialismComponentViewModel
    {
        public PrsSpecialismComponentViewModel()
        {
            SpecialismComponentExams = new List<PrsComponentExamViewModel>();
        }

        public string SpecialismComponentDisplayName { get; set; }        
        public IList<PrsComponentExamViewModel> SpecialismComponentExams { get; set; }
        public bool IsSpecialismAssessmentEntryRegistered { get { return SpecialismComponentExams.Any(x => x.AssessmentId > 0); } }
    }
}
