using AutoMapper;
using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PostResultsServiceService : IPostResultsServiceService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        public readonly IPostResultsServiceRepository _postResultsServiceRepository;
        private readonly IRepository<TqPathwayResult> _pathwayResultRepository;
        private readonly IRepository<TqSpecialismResult> _specialismResultRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PostResultsServiceService> _logger;

        public PostResultsServiceService(ResultsAndCertificationConfiguration configuration, IPostResultsServiceRepository postResultsServiceRepository,
            IRepository<TqPathwayResult> pathwayResultRepository,
            IRepository<TqSpecialismResult> specialismResultRepository,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<PostResultsServiceService> logger)
        {
            _configuration = configuration;
            _postResultsServiceRepository = postResultsServiceRepository;
            _pathwayResultRepository = pathwayResultRepository;
            _specialismResultRepository = specialismResultRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null)
        {
            return await _postResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln, profileId);
        }

        public async Task<bool> PrsActivityAsync(PrsActivityRequest request)
        {
            if (request.PrsStatus == PrsStatus.NotSpecified)
                return false;

            return request.ComponentType switch
            {
                ComponentType.Core => await ProcessPathwayResult(request),
                ComponentType.Specialism => await ProcessSpecialismResult(request),
                _ => false,
            };
        }

        private async Task<bool> ProcessPathwayResult(PrsActivityRequest request)
        {
            var existingPathwayResult = await _pathwayResultRepository.GetFirstOrDefaultAsync(pr => pr.Id == request.ResultId && pr.EndDate == null && pr.IsOptedin
                                                                                     && pr.TqPathwayAssessment.EndDate == null && pr.IsOptedin
                                                                                     && pr.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                                     && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                     && pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn,
                                                                                     navigationPropertyPath: nav => nav.TlLookup);

            if (existingPathwayResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Pathway Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
                return false;
            }

            if (!IsResultStatusValid(request.PrsStatus, existingPathwayResult.PrsStatus))
            {
                _logger.LogWarning(LogEvent.StateChanged, $"Requested status: {request.PrsStatus} is not valid. Current Prs status = {existingPathwayResult.PrsStatus}, ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
                return false;
            }

            if (!IsValidGradeForRommJourney(existingPathwayResult.TlLookup.Code, ComponentType.Core))
            {
                _logger.LogWarning(LogEvent.NotValidData, $"Requested status: {request.PrsStatus} is not valid. Current core result = {existingPathwayResult.TlLookup.Code}, Current Prs status = {existingPathwayResult.PrsStatus}, ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
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
                PrsStatus = GetPrsStatus(request.PrsStatus, existingPathwayResult.PrsStatus),
                IsOptedin = true,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                IsBulkUpload = false,
                CreatedBy = request.PerformedBy
            });

            return await _pathwayResultRepository.UpdateManyAsync(pathwayResultsToUpdate) > 0;
        }

        private async Task<bool> ProcessSpecialismResult(PrsActivityRequest request)
        {
            var existingSpecialismResult = await _specialismResultRepository.GetFirstOrDefaultAsync(sr => sr.Id == request.ResultId && sr.EndDate == null && sr.IsOptedin
                                                                                     && sr.TqSpecialismAssessment.EndDate == null && sr.IsOptedin
                                                                                     && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                                     && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                     && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn,
                                                                                     navigationPropertyPath: nav => nav.TlLookup);

            if (existingSpecialismResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Specialism Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
                return false;
            }

            if (!IsResultStatusValid(request.PrsStatus, existingSpecialismResult.PrsStatus))
            {
                _logger.LogWarning(LogEvent.StateChanged, $"Requested status: {request.PrsStatus} is not valid. Current result status = {existingSpecialismResult.PrsStatus}, ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
                return false;
            }

            if (!IsValidGradeForRommJourney(existingSpecialismResult.TlLookup.Code, ComponentType.Specialism))
            {
                _logger.LogWarning(LogEvent.NotValidData, $"Requested status: {request.PrsStatus} is not valid. Current specialism result = {existingSpecialismResult.TlLookup.Code}, Current Prs status = {existingSpecialismResult.PrsStatus}, ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: PrsActivityAsync({request})");
                return false;
            }

            var specialismResultsToUpdate = new List<TqSpecialismResult>();

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = DateTime.UtcNow;
            existingSpecialismResult.ModifiedBy = request.PerformedBy;
            existingSpecialismResult.ModifiedOn = DateTime.UtcNow;

            specialismResultsToUpdate.Add(existingSpecialismResult);

            var resultLookupId = request.PrsStatus == PrsStatus.UnderReview || request.PrsStatus == PrsStatus.BeingAppealed || request.PrsStatus == PrsStatus.Withdraw
                ? existingSpecialismResult.TlLookupId
                : request.ResultLookupId;

            specialismResultsToUpdate.Add(new TqSpecialismResult
            {
                TqSpecialismAssessmentId = existingSpecialismResult.TqSpecialismAssessmentId,
                TlLookupId = resultLookupId,
                PrsStatus = GetPrsStatus(request.PrsStatus, existingSpecialismResult.PrsStatus),
                IsOptedin = true,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                IsBulkUpload = false,
                CreatedBy = request.PerformedBy
            });

            return await _specialismResultRepository.UpdateManyAsync(specialismResultsToUpdate) > 0;
        }

        public async Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request)
        {
            var referenceNumber = Guid.NewGuid().ToString();

            var emailSent = await SendEmailToTechnicalSupport(referenceNumber, request);
            if (emailSent)
            {
                var userTokens = new Dictionary<string, dynamic>
                {
                    { "reference_number", referenceNumber }
                };

                // send email to requested user
                return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.GradeChangeRequestUserNotification.ToString(), request.RequestedUserEmailAddress, userTokens);
            }
            return false;
        }

        private Task<bool> SendEmailToTechnicalSupport(string referenceNumber, PrsGradeChangeRequest request)
        {
            Dictionary<ComponentType, (string Template, string ComponentTypeToken)> componentTypeDict = new()
            {
                [ComponentType.Core] = (NotificationTemplateName.GradeChangeRequestTechnicalTeamNotificationCoreComponent.ToString(), "core_component"),
                [ComponentType.Specialism] = (NotificationTemplateName.GradeChangeRequestTechnicalTeamNotificationSpecialism.ToString(), "specialism")
            };

            var (template, token) = componentTypeDict[request.ComponentType];

            var technicalTeamTokens = new Dictionary<string, dynamic>
            {
                { "reference_number", referenceNumber },
                { "sender_email_address", request.RequestedUserEmailAddress },
                { "learner_name", request.LearnerName },
                { "uln", request.Uln },
                { "provider_ukprn", request.ProviderUkprn },
                { token, request.ComponentName },
                { "exam_period", request.ExamPeriod },
                { "grade", request.Grade },
                { "requested_message", request.RequestedMessage }
            };

            // send email to technical team
            return _notificationService.SendEmailNotificationAsync(template, _configuration.TechnicalSupportEmailAddress, technicalTeamTokens);
        }

        private static bool IsResultStatusValid(PrsStatus requestPrsStatus, PrsStatus? currentPrsStatus)
        {
            if (requestPrsStatus == PrsStatus.UnderReview)
                return currentPrsStatus == null || currentPrsStatus == PrsStatus.NotSpecified;

            if (requestPrsStatus == PrsStatus.Reviewed)
                return currentPrsStatus == null || currentPrsStatus == PrsStatus.NotSpecified || currentPrsStatus == PrsStatus.UnderReview;

            if (requestPrsStatus == PrsStatus.BeingAppealed)
                return currentPrsStatus == PrsStatus.Reviewed;

            if (requestPrsStatus == PrsStatus.Final)
                return currentPrsStatus == PrsStatus.Reviewed || currentPrsStatus == PrsStatus.BeingAppealed;

            if (requestPrsStatus == PrsStatus.Withdraw)
                return currentPrsStatus == PrsStatus.UnderReview || currentPrsStatus == PrsStatus.BeingAppealed;

            return false;
        }

        private static PrsStatus? GetPrsStatus(PrsStatus requestPrsStatus, PrsStatus? currentPrsStatus)
        {
            if (requestPrsStatus == PrsStatus.Withdraw)
            {
                return currentPrsStatus == PrsStatus.BeingAppealed ? PrsStatus.Reviewed : null;
            }
            else
            {
                return requestPrsStatus;
            }
        }

        private static bool IsValidGradeForRommJourney(string gradeCode, ComponentType componentType)
        {
            return (componentType == ComponentType.Core && !gradeCode.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase) ||
                    componentType == ComponentType.Specialism && !gradeCode.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}