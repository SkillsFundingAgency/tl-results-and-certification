using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqRegistrationProfile : BaseEntity
    {
        public TqRegistrationProfile()
        {
            TqRegistrationPathways = new HashSet<TqRegistrationPathway>();
            QualificationAchieved = new HashSet<QualificationAchieved>();
        }

        public long UniqueLearnerNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public bool? IsEnglishAchieved { get; set; }
        public bool? IsMathsAchieved { get; set; }

        // TODO: Assess following 4 prop to be removed?
        public bool? IsLearnerVerified { get; set; }
        public bool? IsEnglishAndMathsAchieved { get; set; }
        public bool? IsSendLearner { get; set; }
        public bool? IsRcFeed { get; set; }
        
        public virtual ICollection<TqRegistrationPathway> TqRegistrationPathways { get; set; }
        public virtual ICollection<QualificationAchieved> QualificationAchieved { get; set; }
    }
}