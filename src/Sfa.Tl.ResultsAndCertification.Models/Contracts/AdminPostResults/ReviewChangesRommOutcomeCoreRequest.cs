﻿namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults
{
    public class ReviewChangesRommOutcomeCoreRequest : AdminPostResultsRequest
    {
        public int PathwayResultId { get; set; }
        public string ExistingGrade { get; set; }
        public int SelectedGradeId { get; set; }
        public string SelectedGrade { get; set; }       
    }
}