﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AddAssessmentEntryRequest
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public ComponentType ComponentType { get; set; }
        public string PerformedBy { get; set; }
    }
}
