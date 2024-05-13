namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults
{
    public class ReviewChangesAppealOutcomeSpecialismRequest : AdminPostResultsRequest
    {
        public int SpecialismResultId { get; set; }
        public string ExistingGrade { get; set; }
        public int SelectedGradeId { get; set; }
        public string SelectedGrade { get; set; }
    }
}