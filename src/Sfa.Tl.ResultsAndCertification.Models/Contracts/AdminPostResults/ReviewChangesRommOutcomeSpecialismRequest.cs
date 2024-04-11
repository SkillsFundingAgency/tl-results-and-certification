namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults
{
    public class ReviewChangesRommOutcomeSpecialismRequest : AdminPostResultsRequest
    {
        public int SpecialismResultId { get; set; }
        public string ExistingGrade { get; set; }
        public int SelectedGradeId { get; set; }
        public string SelectedGrade { get; set; }       
    }
}