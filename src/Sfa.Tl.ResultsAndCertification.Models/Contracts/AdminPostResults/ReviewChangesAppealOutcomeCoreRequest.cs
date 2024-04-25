namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults
{
    public class ReviewChangesAppealOutcomeCoreRequest : AdminPostResultsRequest
    {
        public int PathwayResultId { get; set; }
        public string ExistingGrade { get; set; }
        public int SelectedGradeId { get; set; }
        public string SelectedGrade { get; set; }
    }
}