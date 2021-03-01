using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class QualificationDataProvider
    {
        public static Qualification CreateQualification(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var qualification = new QualificationBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(qualification);
            }
            return qualification;
        }

        public static Qualification CreateQualification(ResultsAndCertificationDbContext _dbContext, Qualification qualification, bool addToDbContext = true)
        {
            if (qualification == null)
            {
                qualification = new QualificationBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(qualification);
            }
            return qualification;
        }

        public static Qualification CreateQualification(ResultsAndCertificationDbContext _dbContext, int qualificationTypeId, int tlLookupId, string code, string title, bool isSendQualification, bool isActive, bool addToDbContext = true)
        {
            var qualification = new Qualification
            {
                QualificationTypeId = qualificationTypeId,
                TlLookupId = tlLookupId,
                Code = code,
                Title = title,
                IsSendQualification = isSendQualification,
                IsActive = isActive,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(qualification);
            }
            return qualification;
        }

        public static Qualification CreateQualification(ResultsAndCertificationDbContext _dbContext, QualificationType qualificationType, TlLookup tlLookup, string code, string title, bool isSendQualification, bool isActive, bool addToDbContext = true)
        {
            if (qualificationType != null && tlLookup != null)
            {
                var qualification = new Qualification
                {
                    QualificationTypeId = qualificationType.Id,
                    QualificationType = qualificationType,
                    TlLookupId = tlLookup.Id,
                    TlLookup = tlLookup,
                    Code = code,
                    Title = title,
                    IsSendQualification = isSendQualification,
                    IsActive = isActive,
                    CreatedBy = "Test User"
                };

                if (addToDbContext)
                {
                    _dbContext.Add(qualification);
                }
                return qualification;
            }
            return null;
        }

        public static IList<Qualification> CreateQualificationList(ResultsAndCertificationDbContext _dbContext, IList<Qualification> qualification = null, bool addToDbContext = true)
        {
            if (qualification == null)
                qualification = new QualificationBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(qualification);
            }
            return qualification;
        }
    }
}
