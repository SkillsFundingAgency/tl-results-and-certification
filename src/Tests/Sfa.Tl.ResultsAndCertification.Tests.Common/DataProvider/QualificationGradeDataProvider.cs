using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class QualificationGradeDataProvider
    {
        public static QualificationGrade CreateQualificationType(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var qualificationGrade = new QualificationGradeBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(qualificationGrade);
            }
            return qualificationGrade;
        }

        public static QualificationGrade CreateTlLookup(ResultsAndCertificationDbContext _dbContext, QualificationGrade qualificationGrade, bool addToDbContext = true)
        {
            if (qualificationGrade == null)
            {
                qualificationGrade = new QualificationGradeBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(qualificationGrade);
            }
            return qualificationGrade;
        }

        public static QualificationGrade CreateTlLookup(ResultsAndCertificationDbContext _dbContext, int qualificationTypeId, string grade, bool isAllowable, bool isActive, bool addToDbContext = true)
        {
            var qualificationGrade = new QualificationGrade
            {
                QualificationTypeId = qualificationTypeId,
                Grade = grade,
                IsAllowable = isAllowable,
                IsActive = isActive,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(qualificationGrade);
            }
            return qualificationGrade;
        }

        public static IList<QualificationGrade> CreateTlLookupList(ResultsAndCertificationDbContext _dbContext, IList<QualificationGrade> qualificationGrade = null, bool addToDbContext = true)
        {
            if (qualificationGrade == null)
                qualificationGrade = new QualificationGradeBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(qualificationGrade);
            }
            return qualificationGrade;
        }
    }
}
