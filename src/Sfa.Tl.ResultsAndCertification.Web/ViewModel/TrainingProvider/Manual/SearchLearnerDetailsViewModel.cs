namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerDetailsViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public string TlevelName { get; set; }
        public string StartYear { get; set; }

        public bool IsStatusCompleted => IsIndustryPlacementAdded;
        public bool IsIndustryPlacementAdded { get; set; }
    }
}