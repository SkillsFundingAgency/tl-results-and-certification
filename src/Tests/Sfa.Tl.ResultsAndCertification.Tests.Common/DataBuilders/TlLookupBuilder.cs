using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlLookupBuilder
    {
        public TlLookup Build() => new TlLookup
        {
            Category = "PathwayComponentGrade",
            Code = "PCG1",
            Value = "A*",
            IsActive = true,
            SortOrder = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<TlLookup> BuildList() => new List<TlLookup>
        {
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG1",
                Value = "A*",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG2",
                Value = "A",
                IsActive = true,
                SortOrder = 2,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG3",
                Value = "B",
                IsActive = true,
                SortOrder = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG4",
                Value = "C",
                IsActive = true,
                SortOrder = 4,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG5",
                Value = "D",
                IsActive = true,
                SortOrder = 5,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG6",
                Value = "E",
                IsActive = true,
                SortOrder = 6,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "PathwayComponentGrade",
                Code = "PCG7",
                Value = "Unclassified",
                IsActive = true,
                SortOrder = 7,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public TlLookup BuildSpecialismResult() => new TlLookup
        {
            Category = "SpecialismComponentGrade",
            Code = "SCG1",
            Value = "Distinction",
            IsActive = true,
            SortOrder = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<TlLookup> BuildSpecialismResultList() => new List<TlLookup>
        {
            new TlLookup
            {
                Category = "SpecialismComponentGrade",
                Code = "SCG1",
                Value = "Distinction",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "SpecialismComponentGrade",
                Code = "SCG2",
                Value = "Merit",
                IsActive = true,
                SortOrder = 2,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
           new TlLookup
            {
                Category = "SpecialismComponentGrade",
                Code = "SCG3",
                Value = "Pass",
                IsActive = true,
                SortOrder = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "SpecialismComponentGrade",
                Code = "SCG4",
                Value = "Unclassified",
                IsActive = true,
                SortOrder = 4,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public TlLookup BuildSubjectType() => new TlLookup
        {
            Category = "Subject",
            Code = "Eng",
            Value = "English",
            IsActive = true,
            SortOrder = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<TlLookup> BuildSubjectTypeList() => new List<TlLookup>
        {
            new TlLookup
            {
                Category = "Subject",
                Code = "Eng",
                Value = "English",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "Subject",
                Code = "Math",
                Value = "Maths",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public TlLookup BuildIpType() => new TlLookup
        {
            Category = "SpecialConsideration",
            Code = "SC",
            Value = "SpecialConsideration",
            IsActive = true,
            SortOrder = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<TlLookup> BuildIpTypeList() => new List<TlLookup>
        {
            new TlLookup
            {
                Category = "SpecialConsideration",
                Code = "SC",
                Value = "SpecialConsideration",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "IndustryPlacementModel",
                Code = "IPM",
                Value = "IndustryPlacementModel",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "TemporaryFlexibility",
                Code = "TF",
                Value = "TemporaryFlexibility",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };

        public TlLookup BuildOverallResult() => new TlLookup
        {
            Category = "OverallGrade",
            Code = "OG1",
            Value = "Distinction",
            IsActive = true,
            SortOrder = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<TlLookup> BuildOverallResultList() => new List<TlLookup>
        {
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG1",
                Value = "Distinction*",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG2",
                Value = "Distinction",
                IsActive = true,
                SortOrder = 2,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
           new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG3",
                Value = "Merit",
                IsActive = true,
                SortOrder = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG4",
                Value = "Pass",
                IsActive = true,
                SortOrder = 4,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG5",
                Value = "Unclassified",
                IsActive = true,
                SortOrder = 5,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG6",
                Value = "Partial achievement",
                IsActive = true,
                SortOrder = 6,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlLookup
            {
                Category = "OverallGrade",
                Code = "OG5",
                Value = "X - no result",
                IsActive = true,
                SortOrder = 5,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };
    }
}
