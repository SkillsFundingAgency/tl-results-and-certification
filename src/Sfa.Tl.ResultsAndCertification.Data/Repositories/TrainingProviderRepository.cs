using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
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

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            var learnerRecords = await (from tqPathway in _dbContext.TqRegistrationPathway
                                        join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                        join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                        join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                        join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                        join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                        orderby tqPathway.CreatedOn descending
                                        let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()
                                        where tlProvider.UkPrn == request.Ukprn
                                        select new SearchLearnerDetail
                                        {
                                            ProfileId = tqProfile.Id,
                                            Uln = tqProfile.UniqueLearnerNumber,
                                            LearnerName = tqProfile.Firstname + " " + tqProfile.Lastname,
                                            AcademicYear = tqPathway.AcademicYear,
                                            TlevelTitle = tlPathway.TlevelTitle,
                                            EnglishStatus = tqProfile.EnglishStatus,
                                            MathsStatus = tqProfile.MathsStatus,
                                            IndustryPlacementStatus = ipRecord != null ? ipRecord.Status : null
                                        })
                                .ToListAsync();

            var response = new PagedResponse<SearchLearnerDetail> { Records = learnerRecords, TotalRecords = learnerRecords.Count };
            return response;
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            var learnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                       join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                       join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                       join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                       join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                       join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                       orderby tqPathway.CreatedOn descending
                                       where tqProfile.UniqueLearnerNumber == uln && tlProvider.UkPrn == providerUkprn
                                       select new FindLearnerRecord
                                       {
                                           ProfileId = tqProfile.Id,
                                           Uln = tqProfile.UniqueLearnerNumber,
                                           Name = tqProfile.Firstname + " " + tqProfile.Lastname,
                                           DateofBirth = tqProfile.DateofBirth,
                                           ProviderName = tlProvider.Name + " (" + tlProvider.UkPrn + ")",
                                           PathwayName = tlPathway.Name + " (" + tlPathway.LarId + ")",
                                           IsLearnerRegistered = tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn,
                                           IsLearnerRecordAdded = tqProfile.IsEnglishAndMathsAchieved.HasValue && tqPathway.IndustryPlacements.Any(),
                                           IsEnglishAndMathsAchieved = tqProfile.IsEnglishAndMathsAchieved ?? false,
                                           IsSendLearner = tqProfile.IsSendLearner,
                                           HasLrsEnglishAndMaths = tqProfile.IsRcFeed == false && tqProfile.QualificationAchieved.Any(),
                                           IsRcFeed = tqProfile.IsRcFeed
                                       })
                                .FirstOrDefaultAsync();
            return learnerRecord;
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var learnerRecordQuerable = from tqPathway in _dbContext.TqRegistrationPathway
                                        join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                        join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                        join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                        join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                        join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                        orderby tqPathway.CreatedOn descending
                                        let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()
                                        where tqProfile.Id == profileId && tlProvider.UkPrn == providerUkprn
                                        select new LearnerRecordDetails
                                        {
                                            ProfileId = tqProfile.Id,
                                            RegistrationPathwayId = tqPathway.Id,
                                            TlPathwayId = tlPathway.Id,
                                            Uln = tqProfile.UniqueLearnerNumber,
                                            Name = tqProfile.Firstname + " " + tqProfile.Lastname,
                                            DateofBirth = tqProfile.DateofBirth,
                                            ProviderName = tlProvider.Name,
                                            ProviderUkprn = tlProvider.UkPrn,
                                            TlevelTitle = tlPathway.TlevelTitle,
                                            AcademicYear = tqPathway.AcademicYear,
                                            AwardingOrganisationName = tqAo.TlAwardingOrganisaton.DisplayName,
                                            MathsStatus = tqProfile.MathsStatus,
                                            EnglishStatus = tqProfile.EnglishStatus,
                                            IsLearnerRegistered = tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn,
                                            IndustryPlacementId = ipRecord != null ? ipRecord.Id : 0,
                                            IndustryPlacementStatus = ipRecord != null ? ipRecord.Status : null
                                        };

            var learnerRecordDetails = pathwayId.HasValue ? await learnerRecordQuerable.FirstOrDefaultAsync(p => p.RegistrationPathwayId == pathwayId) : await learnerRecordQuerable.FirstOrDefaultAsync();

            return learnerRecordDetails;
        }

        public async Task<bool> IsSendConfirmationRequiredAsync(int profileId)
        {
            var achievemnts = await (from qualAchieved in _dbContext.QualificationAchieved
                                     join qual in _dbContext.Qualification on qualAchieved.QualificationId equals qual.Id
                                     join qualGrade in _dbContext.QualificationGrade on qualAchieved.QualificationGradeId equals qualGrade.Id
                                     join lookup in _dbContext.TlLookup on qual.TlLookupId equals lookup.Id
                                     where qualAchieved.TqRegistrationProfileId == profileId && qualAchieved.IsAchieved
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
