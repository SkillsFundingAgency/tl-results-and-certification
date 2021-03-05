using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class QualificationAchievedDataProvider
    {
        public static QualificationAchieved CreateQualificationAchieved(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var qualificationAchieved = new QualificationAchievedBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(qualificationAchieved);
            }
            return qualificationAchieved;
        }

        public static QualificationAchieved CreateQualificationAchieved(ResultsAndCertificationDbContext _dbContext, QualificationAchieved qualificationAchieved, bool addToDbContext = true)
        {
            if (qualificationAchieved == null)
            {
                qualificationAchieved = new QualificationAchievedBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(qualificationAchieved);
            }
            return qualificationAchieved;
        }

        public static QualificationAchieved CreateQualificationAchieved(ResultsAndCertificationDbContext _dbContext, int tqRegistrationProfileId, int qualificationId, int qualificationGradeId, bool isAchieved, bool addToDbContext = true)
        {
            var qualificationAchieved = new QualificationAchieved
            {
                TqRegistrationProfileId = tqRegistrationProfileId,
                QualificationId = qualificationId,
                QualificationGradeId = qualificationGradeId,                
                IsAchieved = isAchieved
            };

            if (addToDbContext)
            {
                _dbContext.Add(qualificationAchieved);
            }
            return qualificationAchieved;
        }

        public static List<QualificationAchieved> CreateQualificationAchieved(ResultsAndCertificationDbContext _dbContext, List<QualificationAchieved> qualificationAchieved, bool addToDbContext = true)
        {
            if (addToDbContext && qualificationAchieved != null && qualificationAchieved.Count > 0)
            {
                _dbContext.AddRange(qualificationAchieved);
            }
            return qualificationAchieved;
        }
    }
}
