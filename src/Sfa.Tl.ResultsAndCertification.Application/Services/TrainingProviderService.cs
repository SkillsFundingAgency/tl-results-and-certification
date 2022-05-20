using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
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
        private readonly ITrainingProviderRepository _trainingProviderRepository;
        private readonly INotificationService _notificationService;
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainingProviderService(IRepository<TqRegistrationProfile> tqRegistrationProfile,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<IndustryPlacement> industryPlacementRepository,
            ITrainingProviderRepository trainingProviderRepository,
            INotificationService notificationService,
            ResultsAndCertificationConfiguration resultsAndCertificationConfiguration,
            IMapper mapper, ILogger<TrainingProviderService> logger)
        {
            _tqRegistrationProfile = tqRegistrationProfile;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _industryPlacementRepository = industryPlacementRepository;
            _trainingProviderRepository = trainingProviderRepository;
            _notificationService = notificationService;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            return await _trainingProviderRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln, bool? evaluateSendConfirmation = false)
        {            
            var latestPathway = await _trainingProviderRepository.FindLearnerRecordAsync(providerUkprn, uln);

            if (latestPathway != null && evaluateSendConfirmation == true && latestPathway.IsRcFeed == false && latestPathway.IsEnglishAndMathsAchieved == true && latestPathway.IsSendLearner == null)
                latestPathway.IsSendConfirmationRequired = await _trainingProviderRepository.IsSendConfirmationRequiredAsync(latestPathway.ProfileId);

            return latestPathway;
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            return await _trainingProviderRepository.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }

        public async Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request)
        {
            var profile = await _tqRegistrationProfile.GetFirstOrDefaultAsync(p => p.Id == request.ProfileId
                                                                    && p.TqRegistrationPathways.Any(pa => pa.TqProvider.TlProvider.UkPrn == request.ProviderUkprn
                                                                    && (pa.Status == RegistrationPathwayStatus.Active || pa.Status == RegistrationPathwayStatus.Withdrawn)));
            if (profile == null ||
                (request.SubjectType == SubjectType.Maths && profile.MathsStatus != null) ||
                (request.SubjectType == SubjectType.English && profile.EnglishStatus != null) ||
                (request.SubjectType == SubjectType.NotSpecified) ||
                (request.SubjectStatus == SubjectStatus.NotSpecified))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No valid record found to for ProfileId = {request.ProfileId}. Method: UpdateLearnerSubjectAsync({request})");
                return false;
            }

            if (request.SubjectType == SubjectType.Maths)
                profile.MathsStatus = request.SubjectStatus;
            if (request.SubjectType == SubjectType.English)
                profile.EnglishStatus = request.SubjectStatus;

            profile.ModifiedOn = DateTime.UtcNow;
            profile.ModifiedBy = request.PerformedBy;
            
            return await _tqRegistrationProfile.UpdateAsync(profile) > 0;
        }
    }
}