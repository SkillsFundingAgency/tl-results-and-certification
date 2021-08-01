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

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null)
        {
            return await _postResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln, profileId);
        }

        public async Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkPrn, int profileId, int assessmentId)
        {
            return await _postResultsServiceRepository.GetPrsLearnerDetailsAsync(aoUkPrn, profileId, assessmentId);
        }

        public async Task<bool> AppealGradeAsync(AppealGradeRequest request)
        {
            if (request.ComponentType != ComponentType.Core || request.PrsStatus == PrsStatus.NotSpecified)
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

            if (!IsResultStatusValid(request.PrsStatus, existingPathwayResult.PrsStatus))
            {
                _logger.LogWarning(LogEvent.StateChanged, $"Requested status: {request.PrsStatus} is not valid. Current result status = {existingPathwayResult.PrsStatus}, ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: AppealGradeAsync({request})");
                return false;
            }

            var pathwayResultsToUpdate = new List<TqPathwayResult>();

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = DateTime.UtcNow;
            existingPathwayResult.ModifiedBy = request.PerformedBy;
            existingPathwayResult.ModifiedOn = DateTime.UtcNow;

            pathwayResultsToUpdate.Add(existingPathwayResult);

            var resultLookupId = request.PrsStatus == PrsStatus.UnderReview || request.PrsStatus == PrsStatus.BeingAppealed || request.PrsStatus == PrsStatus.Withdraw 
                ? existingPathwayResult.TlLookupId 
                : request.ResultLookupId;

            pathwayResultsToUpdate.Add(new TqPathwayResult
            {
                TqPathwayAssessmentId = existingPathwayResult.TqPathwayAssessmentId,
                TlLookupId = resultLookupId,
                PrsStatus = request.PrsStatus == PrsStatus.Withdraw ? (PrsStatus?)null : request.PrsStatus,
                IsOptedin = true,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                IsBulkUpload = false,
                CreatedBy = request.PerformedBy
            });

            return await _pathwayResultRepository.UpdateManyAsync(pathwayResultsToUpdate) > 0;
        }

        private bool IsResultStatusValid(PrsStatus requestPrsStatus, PrsStatus? currentPrsStatus)
        {
            if (requestPrsStatus == PrsStatus.BeingAppealed)
                return currentPrsStatus == null || currentPrsStatus == PrsStatus.NotSpecified;

            if (requestPrsStatus == PrsStatus.Final)
                return currentPrsStatus == PrsStatus.BeingAppealed;

            if (requestPrsStatus == PrsStatus.Withdraw)
                return currentPrsStatus == PrsStatus.BeingAppealed;

            return false;
        }
    }
}
