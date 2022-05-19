namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerDetailsViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public string TlevelTitle { get; set; }
        public string StartYear { get; set; }

        public bool IsStatusCompleted => IsMathsAdded && IsEnglishAdded && IsIndustryPlacementAdded;
        public bool IsIndustryPlacementAdded { get; set; }
        public bool IsMathsAdded { get; set; }
        public bool IsEnglishAdded { get; set; }
    }
}