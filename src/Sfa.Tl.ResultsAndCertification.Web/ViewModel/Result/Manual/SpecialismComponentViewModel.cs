using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class SpecialismComponentViewModel
    {
        public SpecialismComponentViewModel()
        {
            SpecialismComponentExams = new List<ComponentExamViewModel>();
        }

        public string SpecialismComponentDisplayName { get; set; }
        public IList<ComponentExamViewModel> SpecialismComponentExams { get; set; }
    }
}