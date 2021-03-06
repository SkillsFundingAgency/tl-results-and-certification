﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ChangeResultRequest
    {
        public long AoUkprn { get; set; }
        public long Uln { get; set; }
        public int ProfileId { get; set; }
        public int ResultId { get; set; }
        public int? LookupId { get; set; }
        public ComponentType ComponentType { get; set; }
        public string PerformedBy { get; set; }
    }
}
