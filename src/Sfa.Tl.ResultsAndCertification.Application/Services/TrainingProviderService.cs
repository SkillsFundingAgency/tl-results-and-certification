using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<IndustryPlacement> _industryPlacementRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TrainingProviderService(IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<IndustryPlacement> industryPlacementRepository, IMapper mapper, ILogger<TrainingProviderService> logger)
        {
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _industryPlacementRepository = industryPlacementRepository;
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

            if (!IsValidAddLearnerRecordRequest(pathway, request))
                return new AddLearnerRecordResponse { IsSuccess = false };

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

            var status = await _tqRegistrationPathwayRepository.UpdateWithSpecifedCollectionsOnlyAsync(pathway, false, p => p.TqRegistrationProfile, p => p.IndustryPlacements);
            return new AddLearnerRecordResponse { Uln = request.Uln, Name = $"{pathway.TqRegistrationProfile.Firstname} {pathway.TqRegistrationProfile.Lastname}", IsSuccess = status > 0 };
        }

        public async Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest model)
        {
            if (model == null || !model.HasIndustryPlacementChanged)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Model is null or no changes detected to update Learner record for ProfileId = {model.ProfileId}. Method: UpdateLearnerRecordAsync({model})");
                return false;
            }

            bool isSuccess = false;
            
            if (model.HasIndustryPlacementChanged)
            {
                isSuccess = await HandleIndustryPlacementChanges(model);
            }

            return isSuccess;
        }

        private async Task<bool> HandleIndustryPlacementChanges(UpdateLearnerRecordRequest model)
        {
            var industryPlacement = await _industryPlacementRepository.GetFirstOrDefaultAsync(ip => ip.Id == model.IndustryPlacementId
                                                                                        && ip.TqRegistrationPathway.Id == model.RegistrationPathwayId
                                                                                        && ip.TqRegistrationPathway.TqRegistrationProfileId == model.ProfileId
                                                                                        && ip.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == model.Ukprn);
            if (industryPlacement == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update Industry Placement for UniqueLearnerNumber = {model.Uln}. Method: HandleIndustryPlacementChanges()");
                return false;
            }

            _mapper.Map(model, industryPlacement);
            return await _industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, ip => ip.Status, ip => ip.ModifiedOn, ip => ip.ModifiedBy) > 0;
        }

        private bool IsValidAddLearnerRecordRequest(TqRegistrationPathway registrationPathway, AddLearnerRecordRequest request)
        {
            if (!IsValidPathwayStatus(registrationPathway))
                return false;

            var isValidEnglishAndMaths = (request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus == null) || (!request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus != null);
            var isValidIndustryPlacement = request.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified && !registrationPathway.IndustryPlacements.Any();

            return isValidEnglishAndMaths && isValidIndustryPlacement;
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
    }
}