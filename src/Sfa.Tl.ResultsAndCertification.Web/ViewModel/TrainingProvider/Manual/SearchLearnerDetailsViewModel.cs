namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerDetailsViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public string TlevelTitle { get; set; }         // TODO: check if this need to be TlevelName or Title
        public string StartYear { get; set; }

        public bool IsStatusCompleted => IsMathsAdded && IsEnglishAdded && IsIndustryPlacementAdded;
        public bool IsIndustryPlacementAdded => true;   // IndustryPlacementStatus != IpStatus.NotSpecified;
        public bool IsMathsAdded => false;               // MathsStatus != SubjectStatus.NotSpecified;
        public bool IsEnglishAdded => true;             // EnglishStatus != SubjectStatus.NotSpecified;
    }
}
