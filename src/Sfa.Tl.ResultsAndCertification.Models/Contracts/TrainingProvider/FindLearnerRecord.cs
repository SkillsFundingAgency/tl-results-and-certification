﻿using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class FindLearnerRecord
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string PathwayName { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool? IsSendLearner { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool IsSendConfirmationRequired { get; set; }
        public bool? IsRcFeed { get; set; }
    }
}
