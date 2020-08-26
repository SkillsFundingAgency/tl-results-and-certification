namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RegistrationViewModel
    {
        public UlnViewModel Uln { get; set; }
        
        public LearnersNameViewModel LearnersName { get; set; }
        
        public DateofBirthViewModel DateofBirth { get; set; }
        
        public SelectProviderViewModel SelectProvider { get; set; }
        
        public SelectCoreViewModel SelectCore { get; set; }
        
        public SpecialismQuestionViewModel SpecialismQuestion { get; set; }
        
        public SelectSpecialismViewModel SelectSpecialisms { get; set; }
        
        public SelectAcademicYearViewModel SelectAcademicYear { get; set; }

        public bool IsChangeModeAllowed => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectCore != null 
            && SpecialismQuestion != null && ((SpecialismQuestion.HasLearnerDecidedSpecialism == true && SelectSpecialisms != null)
           || (SpecialismQuestion.HasLearnerDecidedSpecialism == false && SelectSpecialisms == null)) && SelectAcademicYear != null;

        public bool IsChangeModeAllowedForProvider => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectAcademicYear != null;

        public bool IsChangeModeAllowedForCore => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectAcademicYear != null;

        public bool IsChangeModeAllowedForSpecialismQuestion => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectCore != null && SelectAcademicYear != null;

        public bool IsChangeModeAllowedForSelectSpecialism => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectCore != null
            && SpecialismQuestion != null && SelectAcademicYear != null;
    }
}
