using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IRepository<TqRegistrationProfile> _tqRegistrationProfile;
        private readonly ITrainingProviderRepository _trainingProviderRepository;
        private readonly ILogger _logger;

        public TrainingProviderService(IRepository<TqRegistrationProfile> tqRegistrationProfile,
            ITrainingProviderRepository trainingProviderRepository,
            ILogger<TrainingProviderService> logger)
        {
            _tqRegistrationProfile = tqRegistrationProfile;
            _trainingProviderRepository = trainingProviderRepository;
            _logger = logger;
        }

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            return await _trainingProviderRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderRepository.FindLearnerRecordAsync(providerUkprn, uln);
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