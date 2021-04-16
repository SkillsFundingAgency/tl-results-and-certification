using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class TrainingProviderRepository : ITrainingProviderRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsSendConfirmationRequiredAsync(int profileId)
        {
            var achievemnts = from qualAchieved in _dbContext.QualificationAchieved
                              join qual in _dbContext.Qualification on qualAchieved.QualificationId equals qual.Id
                              join qualGrade in _dbContext.QualificationGrade on qualAchieved.QualificationGradeId equals qualGrade.Id
                              join lookup in _dbContext.TlLookup on qual.TlLookupId equals lookup.Id
                              where qualAchieved.TqRegistrationProfileId == profileId && qualAchieved.IsAchieved && qual.IsActive && qualGrade.IsActive
                              select new { Subject = lookup.Code, IsSend = qual.IsSendQualification || qualGrade.IsSendGrade };

            var engSendConfirmationRequired = achievemnts.Where(x => x.Subject == "Eng").All(x => x.IsSend);
            var mathsSendConfirmationRequired = achievemnts.Where(x => x.Subject == "Math").All(x => x.IsSend);

            return engSendConfirmationRequired || mathsSendConfirmationRequired;
        }
    }
}
