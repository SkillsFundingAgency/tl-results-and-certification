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
        public SelectSpecialismViewModel SelectSpecialism { get; set; }
        public SelectAcademicYearViewModel SelectAcademicYear { get; set; }

        public bool IsChangeModeAllowed => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectCore != null 
            && SpecialismQuestion != null && ((SpecialismQuestion.HasLearnerDecidedSpecialism == true && SelectSpecialism != null)
           || (SpecialismQuestion.HasLearnerDecidedSpecialism == false && SelectSpecialism == null)) && SelectAcademicYear != null;

        public bool IsChangeModeAllowedForProvider => Uln != null && LearnersName != null && DateofBirth != null && SelectProvider != null && SelectAcademicYear != null;
    }
}
