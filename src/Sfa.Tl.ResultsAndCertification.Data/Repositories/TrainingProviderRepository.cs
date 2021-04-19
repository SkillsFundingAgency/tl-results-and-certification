using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class TrainingProviderRepository : ITrainingProviderRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsSendConfirmationRequiredAsync(int profileId)
        {
            var achievemnts = await (from qualAchieved in _dbContext.QualificationAchieved
                                     join qual in _dbContext.Qualification on qualAchieved.QualificationId equals qual.Id
                                     join qualGrade in _dbContext.QualificationGrade on qualAchieved.QualificationGradeId equals qualGrade.Id
                                     join lookup in _dbContext.TlLookup on qual.TlLookupId equals lookup.Id
                                     where qualAchieved.TqRegistrationProfileId == profileId && qualAchieved.IsAchieved && qual.IsActive && qualGrade.IsActive
                                     select new { Subject = lookup.Code, IsSend = qual.IsSendQualification || qualGrade.IsSendGrade })
                                     .ToListAsync();

            var englishAchievements = achievemnts?.Where(x => x.Subject == "Eng");
            var mathsAchievements = achievemnts?.Where(x => x.Subject == "Math");

            if (!achievemnts.Any() || !englishAchievements.Any() || !mathsAchievements.Any())
                throw new Exception();

            var isEngSendConfirmationRequired = englishAchievements.All(x => x.IsSend);
            var isMathsSendConfirmationRequired = mathsAchievements.All(x => x.IsSend);

            return isEngSendConfirmationRequired || isMathsSendConfirmationRequired;
        }
    }
}
