﻿using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class SpecialismAssessmentDataProvider
    {
        public static TqSpecialismAssessment CreateTqSpecialismAssessment(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var specialismAssessment = new TqSpecialismAssessmentBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(specialismAssessment);
            }
            return specialismAssessment;
        }

        public static TqSpecialismAssessment CreateTqSpecialismAssessment(ResultsAndCertificationDbContext _dbContext, TqSpecialismAssessment specialismAssessment, bool addToDbContext = true)
        {
            if (specialismAssessment == null)
            {
                specialismAssessment = new TqSpecialismAssessmentBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(specialismAssessment);
            }
            return specialismAssessment;
        }

        public static TqSpecialismAssessment CreateTqSpecialismAssessment(ResultsAndCertificationDbContext _dbContext, int tqRegistrationSpecialismId, int assessmentSeriesId, DateTime startDate, bool addToDbContext = true)
        {
            var specialismAssessment = new TqSpecialismAssessment
            {
                TqRegistrationSpecialismId = tqRegistrationSpecialismId,
                AssessmentSeriesId = assessmentSeriesId,
                StartDate = startDate,
                IsOptedin = true,
                IsBulkUpload = false
            };

            if (addToDbContext)
            {
                _dbContext.Add(specialismAssessment);
            }
            return specialismAssessment;
        }

        public static List<TqSpecialismAssessment> CreateTqSpecialismAssessments(ResultsAndCertificationDbContext _dbContext, List<TqSpecialismAssessment> specialismAssessments, bool addToDbContext = true)
        {
            if (addToDbContext && specialismAssessments != null && specialismAssessments.Count > 0)
            {
                _dbContext.AddRange(specialismAssessments);
            }
            return specialismAssessments;
        }
    }
}
