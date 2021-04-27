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
    }
}
