﻿using Sfa.Tl.ResultsAndCertification.Domain.Models;
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
            Name = "Summer 2021",
            Description = "Summer 2021",
            Year = 2021,
            StartDate = CurrentDate.AddDays(-1),
            EndDate = CurrentDate.AddMonths(3),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<AssessmentSeries> BuildList() => new List<AssessmentSeries>
        {
            new AssessmentSeries
            {
                Name = "Summer 2021",
                Description = "Summer 2021",
                Year = 2021,
                StartDate = CurrentDate.AddDays(-1),
                EndDate = CurrentDate.AddMonths(3),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                Name = "Autumn 2021",
                Description = "Autumn 2021",
                Year = 2021,
                StartDate = CurrentDate.AddMonths(3).AddDays(1),
                EndDate = CurrentDate.AddMonths(6),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                Name = "Summer 2022",
                Description = "Summer 2022",
                Year = 2022,
                StartDate = CurrentDate.AddMonths(6).AddDays(1),
                EndDate = CurrentDate.AddMonths(9),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new AssessmentSeries
            {
                Name = "Autumn 2022",
                Description = "Autumn 2022",
                Year = 2022,
                StartDate = CurrentDate.AddMonths(9).AddDays(1),
                EndDate = CurrentDate.AddMonths(12),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
