using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class AssessmentSeriesBuilder
    {
        private readonly DateTime CurrentDate = DateTime.Now.Date;

        public AssessmentSeries Build() => new AssessmentSeries
        {
            ComponentType = ComponentType.Core,
            Name = "Summer 2021",
            Description = "Summer 2021",
            Year = 2021,
            StartDate = CurrentDate.AddDays(-1),
            EndDate = CurrentDate.AddMonths(3),
            AppealEndDate = CurrentDate.AddMonths(4),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<AssessmentSeries> BuildList() => new List<AssessmentSeries>
        {
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Summer 2021",
                Description = "Summer 2021",
                Year = 2021,
                StartDate = CurrentDate.AddDays(-1),
                EndDate = CurrentDate.AddMonths(3),
                AppealEndDate = CurrentDate.AddMonths(4),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Autumn 2021",
                Description = "Autumn 2021",
                Year = 2021,
                StartDate = CurrentDate.AddMonths(3).AddDays(1),
                EndDate = CurrentDate.AddMonths(6),
                AppealEndDate = CurrentDate.AddMonths(7),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Summer 2022",
                Description = "Summer 2022",
                Year = 2022,
                StartDate = CurrentDate.AddMonths(6).AddDays(1),
                EndDate = CurrentDate.AddMonths(9),
                AppealEndDate = CurrentDate.AddMonths(10),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Autumn 2022",
                Description = "Autumn 2021",
                Year = 2022,
                StartDate = CurrentDate.AddMonths(9).AddDays(1),
                EndDate = CurrentDate.AddMonths(12),
                AppealEndDate = CurrentDate.AddMonths(13),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Summer 2023",
                Description = "Summer 2023",
                Year = 2023,
                StartDate = CurrentDate.AddMonths(12).AddDays(1),
                EndDate = CurrentDate.AddMonths(15),
                AppealEndDate = CurrentDate.AddMonths(16),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Autumn 2023",
                Description = "Autumn 2023",
                Year = 2023,
                StartDate = CurrentDate.AddMonths(15).AddDays(1),
                EndDate = CurrentDate.AddMonths(18),
                AppealEndDate = CurrentDate.AddMonths(19),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
