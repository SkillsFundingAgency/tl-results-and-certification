using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class AddLearnerRecordViewModel
    {
        // TODO: VM will be deleted.-- Also classess inside can be deleted, assess.
        public FindLearnerRecord LearnerRecord { get; set; }
        public EnterUlnViewModel Uln { get; set; }
        //public EnglishAndMathsQuestionViewModel EnglishAndMathsQuestion { get; set; }
        public EnglishAndMathsLrsQuestionViewModel EnglishAndMathsLrsQuestion { get; set; }

        public bool IsChangeModeAllowed => true;
    }
}
