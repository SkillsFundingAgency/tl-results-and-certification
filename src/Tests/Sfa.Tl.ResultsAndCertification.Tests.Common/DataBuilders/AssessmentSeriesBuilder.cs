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
            RommEndDate = CurrentDate.AddMonths(4),
            AppealEndDate = CurrentDate.AddMonths(5),
            ResultCalculationYear = 2020,
            ResultPublishDate = CurrentDate.AddMonths(6),
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
                RommEndDate = CurrentDate.AddMonths(4),
                AppealEndDate = CurrentDate.AddMonths(5),
                ResultCalculationYear = 2020,
                ResultPublishDate= CurrentDate.AddMonths(6),
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
                RommEndDate = CurrentDate.AddMonths(7),
                AppealEndDate = CurrentDate.AddMonths(8),
                ResultCalculationYear = 2020,
                ResultPublishDate  = CurrentDate.AddMonths(9),
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
                RommEndDate = CurrentDate.AddMonths(10),
                AppealEndDate = CurrentDate.AddMonths(11),
                ResultCalculationYear = 2020,
                ResultPublishDate = CurrentDate.AddMonths(12),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Core,
                Name = "Autumn 2022",
                Description = "Autumn 2022",
                Year = 2022,                
                StartDate = CurrentDate.AddMonths(9).AddDays(1),
                EndDate = CurrentDate.AddMonths(12),
                RommEndDate = CurrentDate.AddMonths(13),
                AppealEndDate = CurrentDate.AddMonths(14),
                ResultCalculationYear = 2020,
                ResultPublishDate = CurrentDate.AddMonths(15),
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
                RommEndDate = CurrentDate.AddMonths(16),
                AppealEndDate = CurrentDate.AddMonths(17),
                ResultCalculationYear = 2021,
                ResultPublishDate = CurrentDate.AddMonths(18),
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
                RommEndDate = CurrentDate.AddMonths(19),
                AppealEndDate = CurrentDate.AddMonths(20),
                ResultCalculationYear = 2021,
                ResultPublishDate = CurrentDate.AddMonths(21),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2022",
                Description = "Summer 2022",
                Year = 2022,                
                StartDate = CurrentDate.AddDays(-1),
                EndDate = CurrentDate.AddMonths(3),
                RommEndDate = CurrentDate.AddMonths(4),
                AppealEndDate = CurrentDate.AddMonths(5),
                ResultCalculationYear = null,
                ResultPublishDate = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2023",
                Description = "Summer 2023",
                Year = 2023,                
                StartDate = CurrentDate.AddYears(1).AddDays(-1),
                EndDate = CurrentDate.AddYears(1).AddMonths(3),
                RommEndDate = CurrentDate.AddYears(1).AddMonths(4),
                AppealEndDate = CurrentDate.AddYears(1).AddMonths(5),
                ResultCalculationYear = null,
                ResultPublishDate = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2024",
                Description = "Summer 2024",
                Year = 2024,                
                StartDate = CurrentDate.AddYears(2).AddDays(-1),
                EndDate = CurrentDate.AddYears(2).AddMonths(3),
                RommEndDate = CurrentDate.AddYears(3).AddMonths(4),
                AppealEndDate = CurrentDate.AddYears(3).AddMonths(5),
                ResultCalculationYear = null,
                ResultPublishDate = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
