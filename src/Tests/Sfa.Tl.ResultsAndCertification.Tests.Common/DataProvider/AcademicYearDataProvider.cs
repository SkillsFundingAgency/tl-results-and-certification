using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class AcademicYearDataProvider
    {
        public static AcademicYear CreateAcademicYear(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var academicYear = new AcademicYearBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(academicYear);
            }
            return academicYear;
        }

        public static AcademicYear CreateAcademicYear(ResultsAndCertificationDbContext _dbContext, AcademicYear academicYear, bool addToDbContext = true)
        {
            if (academicYear == null)
            {
                academicYear = new AcademicYearBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(academicYear);
            }
            return academicYear;
        }

        public static AcademicYear CreateAcademicYear(ResultsAndCertificationDbContext _dbContext, string name, int year, DateTime startDate, DateTime endDate, bool addToDbContext = true)
        {
            var academicYear = new AcademicYear
            {
                Name = name,                
                Year = year,
                StartDate = startDate,
                EndDate = endDate,                
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(academicYear);
            }
            return academicYear;
        }

        public static IList<AcademicYear> CreateAcademicYearList(ResultsAndCertificationDbContext _dbContext, IList<AcademicYear> academicYears, bool addToDbContext = true)
        {
            if (academicYears == null)
                academicYears = new AcademicYearBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(academicYears);
            }
            return academicYears;
        }
    }
}
