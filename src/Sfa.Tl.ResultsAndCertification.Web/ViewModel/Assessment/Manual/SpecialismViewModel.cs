using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class SpecialismViewModel
    {
        public int Id { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<SpecialismAssessmentViewModel> Assessments { get; set; }
    }
}
