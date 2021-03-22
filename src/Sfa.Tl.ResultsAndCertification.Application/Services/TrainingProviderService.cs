using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
        private readonly IMapper _mapper;

        public TrainingProviderService(IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository, IMapper mapper)
        {
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _mapper = mapper;
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

            if(IsValidAddEnglishAndMathsRequest(pathway, request))
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

        private bool IsValidAddLearnerRecordRequest(TqRegistrationPathway registrationPathway, AddLearnerRecordRequest request)
        {
            if (registrationPathway == null)
                return false;

            var isValidEnglishAndMaths = (request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus == null) || (!request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus != null);
            var isValidIndustryPlacement = request.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified && !registrationPathway.IndustryPlacements.Any();

            return isValidEnglishAndMaths && isValidIndustryPlacement;
        }

        private bool IsValidAddEnglishAndMathsRequest(TqRegistrationPathway registrationPathway, AddLearnerRecordRequest request)
        {
            if (registrationPathway == null)
                return false;

            return  !request.HasLrsEnglishAndMaths && request.EnglishAndMathsStatus != null 
                && registrationPathway.TqRegistrationProfile.IsEnglishAndMathsAchieved == null 
                && registrationPathway.TqRegistrationProfile.IsRcFeed == null;            
        }
    }
}