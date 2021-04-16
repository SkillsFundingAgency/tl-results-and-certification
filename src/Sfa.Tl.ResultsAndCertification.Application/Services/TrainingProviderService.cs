using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IRepository<TqRegistrationProfile> _tqRegistrationProfile;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<IndustryPlacement> _industryPlacementRepository;
        private readonly INotificationService _notificationService;
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainingProviderService(IRepository<TqRegistrationProfile> tqRegistrationProfile, 
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<IndustryPlacement> industryPlacementRepository,
            INotificationService notificationService,
            ResultsAndCertificationConfiguration resultsAndCertificationConfiguration,
            IMapper mapper, ILogger<TrainingProviderService> logger)
        {
            _tqRegistrationProfile = tqRegistrationProfile;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _industryPlacementRepository = industryPlacementRepository;
            _notificationService = notificationService;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            var latestPathway = await _tqRegistrationPathwayRepository
                                    .GetManyAsync(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln &&
                                        x.TqProvider.TlProvider.UkPrn == providerUkprn,
                                        navigationPropertyPath: new Expression<Func<TqRegistrationPathway, object>>[]
                                        {
                                            n => n.TqRegistrationProfile,
                                            n => n.TqProvider.TlProvider,
                                            n => n.TqProvider.TqAwardingOrganisation.TlPathway,
                                            n => n.IndustryPlacements
                                        })
                                    .Include(x => x.TqRegistrationProfile.QualificationAchieved).ThenInclude(x => x.Qualification)
                                    .OrderByDescending(o => o.CreatedOn)
                                    .FirstOrDefaultAsync();

            return _mapper.Map<FindLearnerRecord>(latestPathway);
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var latestPathwayQuerable = _tqRegistrationPathwayRepository
                                        .GetManyAsync(x => x.TqRegistrationProfile.Id == profileId &&
                                            x.TqProvider.TlProvider.UkPrn == providerUkprn,
                                            navigationPropertyPath: new Expression<Func<TqRegistrationPathway, object>>[]
                                            {
                                                n => n.TqRegistrationProfile,
                                                n => n.TqProvider.TlProvider,
                                                n => n.TqProvider.TqAwardingOrganisation.TlPathway,
                                                n => n.IndustryPlacements
                                            })
                                        .Include(x => x.TqRegistrationProfile.QualificationAchieved).ThenInclude(x => x.Qualification)
                                        .OrderByDescending(o => o.CreatedOn);

            var latestPathway = pathwayId.HasValue ? await latestPathwayQuerable.FirstOrDefaultAsync(p => p.Id == pathwayId) : await latestPathwayQuerable.FirstOrDefaultAsync();
            return _mapper.Map<LearnerRecordDetails>(latestPathway);
        }

        public async Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request)
        {
            var pathway = await _tqRegistrationPathwayRepository
                                    .GetManyAsync(x => x.TqRegistrationProfile.UniqueLearnerNumber == request.Uln &&
                                        x.TqProvider.TlProvider.UkPrn == request.Ukprn,
                                        navigationPropertyPath: new Expression<Func<TqRegistrationPathway, object>>[]
                                        {
                                            n => n.TqRegistrationProfile,
                                            n => n.IndustryPlacements
                                        })
                                    .OrderByDescending(o => o.CreatedOn)
                                    .FirstOrDefaultAsync();

            var isSuccess = false;

            if (!IsValidAddLearnerRecordRequest(pathway, request))
                return new AddLearnerRecordResponse { IsSuccess = isSuccess };
            
            if (IsValidEnglishAndMathsLrsEmailRequest(request))
            {
                isSuccess = await SendEmailAsync(pathway.TqRegistrationProfileId, request);
            }
            else
            {
                if (IsValidAddEnglishAndMathsRequest(pathway, request))
                {
                    pathway.TqRegistrationProfile.IsEnglishAndMathsAchieved = request.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.Achieved || request.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.AchievedWithSend;
                    pathway.TqRegistrationProfile.IsSendLearner = request.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.AchievedWithSend ? true : (bool?)null;
                    pathway.TqRegistrationProfile.IsRcFeed = true;
                    pathway.TqRegistrationProfile.ModifiedBy = request.PerformedBy;
                    pathway.TqRegistrationProfile.ModifiedOn = DateTime.UtcNow;
                }

                pathway.IndustryPlacements.Add(new IndustryPlacement
                {
                    TqRegistrationPathwayId = pathway.Id,
                    Status = request.IndustryPlacementStatus,
                    CreatedBy = request.PerformedBy
                });

                isSuccess = await _tqRegistrationPathwayRepository.UpdateWithSpecifedCollectionsOnlyAsync(pathway, false, p => p.TqRegistrationProfile, p => p.IndustryPlacements) > 0;
            }
            return new AddLearnerRecordResponse { Uln = request.Uln, Name = $"{pathway.TqRegistrationProfile.Firstname} {pathway.TqRegistrationProfile.Lastname}", IsSuccess = isSuccess };
        }

        public async Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest request)
        {
            if (request == null || (!request.HasEnglishAndMathsChanged && !request.HasIndustryPlacementChanged))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Model is null or no changes detected to update Learner record for Uln = {request.Uln}. Method: UpdateLearnerRecordAsync({request})");
                return false;
            }

            bool isSuccess = false;

            if (request.HasEnglishAndMathsChanged)
            {
                isSuccess = await HandleEnglishAndMathsChanges(request);
            }
            else if (request.HasIndustryPlacementChanged)
            {
                isSuccess = await HandleIndustryPlacementChanges(request);
            }

            return isSuccess;
        }

        private async Task<bool> HandleEnglishAndMathsChanges(UpdateLearnerRecordRequest request)
        {
            var profile = await _tqRegistrationProfile.GetFirstOrDefaultAsync(p => p.Id == request.ProfileId && p.UniqueLearnerNumber == request.Uln
                                                                    && p.IsEnglishAndMathsAchieved.HasValue && p.IsRcFeed == true
                                                                    && p.TqRegistrationPathways.Any(pa => pa.TqProvider.TlProvider.UkPrn == request.Ukprn
                                                                    && (pa.Status == RegistrationPathwayStatus.Active || pa.Status == RegistrationPathwayStatus.Withdrawn)));

            if (profile == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update English and Maths for Uln = {request.Uln}. Method: HandleEnglishAndMathsChanges({request})");
                return false;
            }

            _mapper.Map(request, profile);
            return await _tqRegistrationProfile.UpdateWithSpecifedColumnsOnlyAsync(profile, p => p.IsEnglishAndMathsAchieved, p => p.IsSendLearner, p => p.ModifiedOn, p => p.ModifiedBy) > 0;
        }

        private async Task<bool> HandleIndustryPlacementChanges(UpdateLearnerRecordRequest request)
        {
            var industryPlacement = await _industryPlacementRepository.GetFirstOrDefaultAsync(ip => ip.Id == request.IndustryPlacementId
                                                                                    && ip.TqRegistrationPathway.Id == request.RegistrationPathwayId
                                                                                    && ip.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                                    && ip.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == request.Ukprn
                                                                                    && (ip.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                    || ip.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn));
            if (industryPlacement == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update Industry Placement for Uln = {request.Uln}. Method: HandleIndustryPlacementChanges({request})");
                return false;
            }

            _mapper.Map(request, industryPlacement);
            return await _industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, ip => ip.Status, ip => ip.ModifiedOn, ip => ip.ModifiedBy) > 0;
        }        

        private bool IsValidAddLearnerRecordRequest(TqRegistrationPathway registrationPathway, AddLearnerRecordRequest request)
        {
            if (!IsValidPathwayStatus(registrationPathway))
                return false;

            var isValidEnglishAndMathsLrsEmailRequest = IsValidEnglishAndMathsLrsEmailRequest(request);

            var isValidEnglishAndMaths = isValidEnglishAndMathsLrsEmailRequest || (request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus == null) 
                || (!request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus != null && request.EnglishAndMathsLrsStatus == null);

            var isValidIndustryPlacement = request.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified && !registrationPathway.IndustryPlacements.Any();

            return isValidEnglishAndMathsLrsEmailRequest ? isValidEnglishAndMaths : isValidEnglishAndMaths && isValidIndustryPlacement;
        }

        private bool IsValidEnglishAndMathsLrsEmailRequest(AddLearnerRecordRequest request)
        {
            return request != null && request.HasLrsEnglishAndMaths && request.EnglishAndMathsLrsStatus != null && request.EnglishAndMathsStatus == null;
        }

        private bool IsValidAddEnglishAndMathsRequest(TqRegistrationPathway registrationPathway, AddLearnerRecordRequest request)
        {
            return !request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus != null
                && registrationPathway.TqRegistrationProfile.IsEnglishAndMathsAchieved == null
                && registrationPathway.TqRegistrationProfile.IsRcFeed == null;
        }

        private bool IsValidPathwayStatus(TqRegistrationPathway registrationPathway)
        {
            return registrationPathway?.Status == RegistrationPathwayStatus.Active || registrationPathway?.Status == RegistrationPathwayStatus.Withdrawn;
        }

        private async Task<bool> SendEmailAsync(int profileId, AddLearnerRecordRequest request)
        {
            var tokens = new Dictionary<string, dynamic>
                {
                    { "profile_id", profileId },
                    { "english_and_maths_lrs_status", request.EnglishAndMathsLrsStatus.ToString() },
                    { "sender_name", request.PerformedBy },
                    { "sender_email_address", request.PerformedUserEmail }
                };

            return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.EnglishAndMathsLrsDataQueried.ToString(), _resultsAndCertificationConfiguration.TlevelQueriedSupportEmailAddress, tokens);
        }
    }
}