using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class AddLearnerRecordViewModel
    {
        public FindLearnerRecord LearnerRecord { get; set; }
        public EnterUlnViewModel Uln { get; set; }
        public IndustryPlacementQuestionViewModel IndustryPlacementQuestion{ get; set; }
    }
}
