using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class QualificationTypeDataProvider
    {
        public static QualificationType CreateQualificationType(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var qualificationType = new QualificationTypeBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(qualificationType);
            }
            return qualificationType;
        }

        public static QualificationType CreateTlLookup(ResultsAndCertificationDbContext _dbContext, QualificationType qualificationType, bool addToDbContext = true)
        {
            if (qualificationType == null)
            {
                qualificationType = new QualificationTypeBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(qualificationType);
            }
            return qualificationType;
        }

        public static QualificationType CreateTlLookup(ResultsAndCertificationDbContext _dbContext, string name, string subTitle, bool isActive, bool addToDbContext = true)
        {
            var qualificationType = new QualificationType
            {
                Name = name,
                SubTitle = subTitle,
                IsActive = isActive,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(qualificationType);
            }
            return qualificationType;
        }

        public static IList<QualificationType> CreateTlLookupList(ResultsAndCertificationDbContext _dbContext, IList<QualificationType> qualificationType = null, bool addToDbContext = true)
        {
            if (qualificationType == null)
                qualificationType = new QualificationTypeBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(qualificationType);
            }
            return qualificationType;
        }
    }
}
