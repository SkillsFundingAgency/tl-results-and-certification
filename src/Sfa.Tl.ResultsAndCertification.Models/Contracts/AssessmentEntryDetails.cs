﻿namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AssessmentEntryDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public int AssessmentId { get; set; }
        public int AssessmentSeriesId { get; set; }

        public string AssessmentSeriesName { get; set; }
    }
}
