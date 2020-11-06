using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class AssessmentSeriesBuilder
    {
        public AssessmentSeries Build() => new AssessmentSeries
        {
            Name = "Summer 2021",
            Description = "Summer 2021",
            Year = 2021,
            EndDate = DateTime.Now.AddDays(90),
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
                EndDate = DateTime.Now.AddDays(90),
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
                EndDate = DateTime.Now.AddDays(90),
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
                EndDate = DateTime.Now.AddDays(90),
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
                EndDate = DateTime.Now.AddDays(90),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
