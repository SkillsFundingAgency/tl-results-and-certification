using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class AcademicYearBuilder
    {
        public AcademicYear Build() => new AcademicYear
        {
            Name = "2020/21",
            Year = 2020,
            StartDate = "2020/09/01".ToDateTime(),
            EndDate = "2021/08/31".ToDateTime(),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<AcademicYear> BuildList() => new List<AcademicYear>
        {
            new AcademicYear
            {
                Name = "2020/21",                
                Year = 2020,
                StartDate = "2020/09/01".ToDateTime(),
                EndDate = "2021/08/31".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AcademicYear
            {
                Name = "2021/22",
                Year = 2021,
                StartDate = "2021/09/01".ToDateTime(),
                EndDate = "2022/08/31".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AcademicYear
            {
                Name = "2022/23",
                Year = 2022,
                StartDate = "2022/09/01".ToDateTime(),
                EndDate = "2023/08/31".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AcademicYear
            {
                Name = "2023/24",
                Year = 2023,
                StartDate = "2023/09/01".ToDateTime(),
                EndDate = "2024/08/31".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AcademicYear
            {
                Name = "2024/25",
                Year = 2024,
                StartDate = "2024/09/01".ToDateTime(),
                EndDate = "2025/08/31".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
