﻿namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AddResultResponse
    {
        public long Uln { get; set; }
        public int ProfileId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
