
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
        public bool IsChangeModeAllowedForSpecialismQuestion => ReregisterProvider != null && ReregisterCore != null && ReregisterAcademicYear != null;
        public bool IsChangeModeAllowedForSelectSpecialism => ReregisterProvider != null && ReregisterCore != null
            && SpecialismQuestion != null && ReregisterAcademicYear != null;
    }
}
