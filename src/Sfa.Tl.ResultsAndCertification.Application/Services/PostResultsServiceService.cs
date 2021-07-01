using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PostResultsServiceService : IPostResultsServiceService
    {
        public readonly IPostResultsServiceRepository _postResultsServiceRepository;
        private readonly IRepository<TqPathwayResult> _pathwayResultRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostResultsServiceService> _logger;

        public PostResultsServiceService(IPostResultsServiceRepository postResultsServiceRepository,
            IRepository<TqPathwayResult> pathwayResultRepository,
            IMapper mapper,
            ILogger<PostResultsServiceService> logger)
        {
            _postResultsServiceRepository = postResultsServiceRepository;
            _pathwayResultRepository = pathwayResultRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _postResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        public async Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkPrn, int profileId, int assessmentId)
        {
            return await _postResultsServiceRepository.GetPrsLearnerDetailsAsync(aoUkPrn, profileId, assessmentId);
        }

        public async Task<bool> AppealGradeAsync(AppealGradeRequest request)
        {
            if (request.ComponentType != ComponentType.Core)
                return false;

            var existingPathwayResult = await _pathwayResultRepository.GetFirstOrDefaultAsync(pr => pr.Id == request.ResultId && pr.EndDate == null && pr.IsOptedin
                                                                         && pr.TqPathwayAssessment.EndDate == null && pr.IsOptedin
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn);

            if (existingPathwayResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Pathway Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: AppealGradeAsync({request})");
                return false;
            }

            var pathwayResultsToUpdate = new List<TqPathwayResult>();

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = DateTime.UtcNow;
            existingPathwayResult.ModifiedBy = request.PerformedBy;
            existingPathwayResult.ModifiedOn = DateTime.UtcNow;

            pathwayResultsToUpdate.Add(existingPathwayResult);

            if (request.PrsStatus > 0)
            {
                pathwayResultsToUpdate.Add(new TqPathwayResult
                {
                    TqPathwayAssessmentId = existingPathwayResult.TqPathwayAssessmentId,
                    TlLookupId = existingPathwayResult.TlLookupId,
                    //PrsStatus = request.PrsStatus,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }

            return await _pathwayResultRepository.UpdateManyAsync(pathwayResultsToUpdate) > 0;
        }
    }
}
