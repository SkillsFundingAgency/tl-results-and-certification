﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminLearnerRecord
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int TlPathwayId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int TlevelStartYear { get; set; }
        public int AcademicYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus? MathsStatus { get; set; }
        public SubjectStatus? EnglishStatus { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public bool IsPendingWithdrawal { get; set; }
        // English and Maths
        public SubjectStatus IsEnglishAchieved { get; set; }
        public SubjectStatus IsMathsAchieved { get; set; }

        // Industry placement
        public int IndustryPlacementId { get; set; }
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }
        public string IndustryPlacementDetails { get; set; }

        public string DisplayAcademicYear { get; set; }
        public List<int> AcademicStartYearsToBe { get; set; }
        
        public CalculationStatus? OverallCalculationStatus { get; set; }
    }
}
