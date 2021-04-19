using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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
        private readonly ILogger<TrainingProviderRepository> _logger;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext, ILogger<TrainingProviderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> IsSendConfirmationRequiredAsync(int profileId)
        {
            var achievemnts = await (from qualAchieved in _dbContext.QualificationAchieved
                                     join qual in _dbContext.Qualification on qualAchieved.QualificationId equals qual.Id
                                     join qualGrade in _dbContext.QualificationGrade on qualAchieved.QualificationGradeId equals qualGrade.Id
                                     join lookup in _dbContext.TlLookup on qual.TlLookupId equals lookup.Id
                                     where qualAchieved.TqRegistrationProfileId == profileId && qualAchieved.IsAchieved && qual.IsActive && qualGrade.IsActive
                                     select new { Subject = lookup.Value, IsSend = qual.IsSendQualification || qualGrade.IsSendGrade })
                                     .ToListAsync();

            var englishAchievements = achievemnts?.Where(x => x.Subject == QualificationSubject.English.ToString());
            var mathsAchievements = achievemnts?.Where(x => x.Subject == QualificationSubject.Maths.ToString());

            if (!englishAchievements.Any() || !mathsAchievements.Any())
            {
                var message = $"Data not supported - both English and Maths achievements are expected. Method: IsSendConfirmationRequiredAsync({profileId}), EnglishAchieved: {englishAchievements.Count()}, MathsAchieved: {mathsAchievements.Count()}";
                _logger.LogInformation(LogEvent.UnSupportedMethod, message);
                throw new Exception(message);
            }

            var isEngSendConfirmationRequired = englishAchievements.All(x => x.IsSend);
            var isMathsSendConfirmationRequired = mathsAchievements.All(x => x.IsSend);

            return isEngSendConfirmationRequired || isMathsSendConfirmationRequired;
        }
    }
}
