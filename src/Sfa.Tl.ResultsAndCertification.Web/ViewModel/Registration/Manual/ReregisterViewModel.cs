
namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterViewModel
    {
        public ReregisterProviderViewModel ReregisterProvider { get; set; }
        public ReregisterCoreViewModel ReregisterCore { get; set; }
        public ReregisterSpecialismQuestionViewModel SpecialismQuestion { get; set; }
        public ReregisterSpecialismViewModel ReregisterSpecialisms { get; set; }
        public ReregisterAcademicYearViewModel ReregisterAcademicYear { get; set; }
        
        public bool IsChangeModeAllowedForProvider => ReregisterAcademicYear != null;
        public bool IsChangeModeAllowedForCore => ReregisterProvider != null && ReregisterAcademicYear != null;

        public bool IsChangeModeAllowed => ReregisterProvider != null && ReregisterCore != null
            && SpecialismQuestion != null && ((SpecialismQuestion.HasLearnerDecidedSpecialism == true && ReregisterSpecialisms != null)
           || (SpecialismQuestion.HasLearnerDecidedSpecialism == false && ReregisterSpecialisms == null)) && ReregisterAcademicYear != null;
    }
}