using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class IndustryPlacementNotificationService : IIndustryPlacementNotificationService
    {
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<NotificationTemplate> _notificationTemplateRepository;
        private readonly IRepository<TlProvider> _tlProviderRepository;
        private readonly IDfeSignInApiClient _dfeSignInApiClient;
        private readonly INotificationService _notificationService;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly ICommonRepository _commonRepository;

        private readonly ILogger _logger;

        public IndustryPlacementNotificationService(
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<NotificationTemplate> notificationTemplateRepository,
            IRepository<TlProvider> tlProviderRepository,
            ICommonRepository commonRepository,
            IDfeSignInApiClient dfeSignInApiClient,
            INotificationService notificationService,
            IAsyncNotificationClient notificationClient,
            ILogger<IndustryPlacementNotificationService> logger)
        {
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _notificationTemplateRepository = notificationTemplateRepository;
            _tlProviderRepository = tlProviderRepository;
            _commonRepository = commonRepository;
            _dfeSignInApiClient = dfeSignInApiClient;
            _notificationService = notificationService;
            _notificationClient = notificationClient;
            _logger = logger;
        }

        public async Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementFirstDeadlineReminderAsync()
        {
            var activeProviders = await _tlProviderRepository.GetManyAsync(p => p.IsActive).Select(p => p.UkPrn).ToListAsync();

            if (activeProviders == null || !activeProviders.Any())
            {
                throw new ApplicationException($"There are no active providers. Method: {nameof(ProcessIndustryPlacementFirstDeadlineReminderAsync)}");
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(activeProviders);

            Dictionary<string, dynamic> userTokens = new()
            {
                { "reference_number", 000000 }
            };

            return await SendEmailNotificationAsync(NotificationTemplateName.IndustryPlacementFirstDeadlineReminder.ToString(), providerUsers, userTokens);
        }

        public async Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementMissedDeadlineReminderAsync()
        {
            IndustryPlacementNotificationResponse response = new();

            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();

            if (currentAcademicYears == null || !currentAcademicYears.Any())
            {
                throw new ApplicationException($"Current Academic years are not found. Method: {nameof(ProcessIndustryPlacementMissedDeadlineReminderAsync)}");
            }

            var previousAcademicYear = currentAcademicYears.FirstOrDefault()?.Year - 1;

            var filteredPathways = await _tqRegistrationPathwayRepository.GetManyAsync()
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                .Where(w => w.Status == RegistrationPathwayStatus.Active &&
                            w.EndDate == null &&
                            w.AcademicYear == previousAcademicYear)
                .ToListAsync();

            var pathwaysWithoutPlacements = filteredPathways
                .Where(w => !w.IndustryPlacements.Any())
                .ToList();

            var ukprnCountDictionary = pathwaysWithoutPlacements
                .GroupBy(g => g.TqProvider.TlProvider.UkPrn)
                .ToDictionary(group => group.Key, group => group.Count());

            if (ukprnCountDictionary == null || !ukprnCountDictionary.Any())
            {
                var message = $"No entries are found. Method: {nameof(ProcessIndustryPlacementMissedDeadlineReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(ukprnCountDictionary.Keys);

            if (providerUsers == null)
            {
                var message = $"No provider users are found. Method: {nameof(ProcessIndustryPlacementMissedDeadlineReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            ukprnCountDictionary.ToList().ForEach(rp =>
            {
                Dictionary<string, dynamic> userTokens = new()
                {
                    { "Number-of-learners", ukprnCountDictionary[rp.Key] }
                };

                var dfeUsers = providerUsers.Where(providerUsers => providerUsers.Ukprn == rp.Key.ToString()).ToList();

                if (dfeUsers == null || !dfeUsers.Any())
                    return;

                var emailSentResponse = SendEmailNotificationAsync(NotificationTemplateName.IndustryPlacementMissedDeadlineReminder.ToString(), dfeUsers, userTokens);

                response.IsSuccess = emailSentResponse.Result.IsSuccess;
                response.UsersCount += emailSentResponse.Result.UsersCount;
                response.EmailSentCount += emailSentResponse.Result.EmailSentCount;
            });

            response.Message = $"Total users: {response.UsersCount} Email sent: {response.EmailSentCount}.";

            return response;
        }

        public async Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementChaseBigGapsReminderAsync()
        {
            IndustryPlacementNotificationResponse response = new();

            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();

            if (currentAcademicYears == null || !currentAcademicYears.Any())
            {
                throw new ApplicationException($"Current Academic years are not found. Method: {nameof(ProcessIndustryPlacementChaseBigGapsReminderAsync)}");
            }

            var previousAcademicYear = currentAcademicYears.FirstOrDefault()?.Year - 1;

            var filteredPathways = await _tqRegistrationPathwayRepository.GetManyAsync()
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                .Where(w => w.Status == RegistrationPathwayStatus.Active &&
                            w.EndDate == null &&
                            w.AcademicYear == previousAcademicYear)
                .ToListAsync();

            var pathwaysWithoutPlacements = filteredPathways
                .Where(w => !w.IndustryPlacements.Any())
                .ToList();

            var ukprnCountDictionary = pathwaysWithoutPlacements
                .GroupBy(g => g.TqProvider.TlProvider.UkPrn)
                .ToDictionary(group => group.Key, group => group.Count());

            if (ukprnCountDictionary == null || !ukprnCountDictionary.Any())
            {
                var message = $"No entries are found. Method: {nameof(ProcessIndustryPlacementChaseBigGapsReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(ukprnCountDictionary.Keys);

            if (providerUsers == null)
            {
                var message = $"No provider users are found. Method: {nameof(ProcessIndustryPlacementChaseBigGapsReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            ukprnCountDictionary.ToList().ForEach(rp =>
            {
                Dictionary<string, dynamic> userTokens = new()
                {
                    { "Number-of-learners", ukprnCountDictionary[rp.Key] }
                };

                var dfeUsers = providerUsers.Where(providerUsers => providerUsers.Ukprn == rp.Key.ToString()).ToList();

                if (dfeUsers == null || !dfeUsers.Any())
                    return;

                var emailSentResponse = SendEmailNotificationAsync(NotificationTemplateName.IndustryPlacementChaseBigGapsReminder.ToString(), dfeUsers, userTokens);

                response.IsSuccess = emailSentResponse.Result.IsSuccess;
                response.UsersCount += emailSentResponse.Result.UsersCount;
                response.EmailSentCount += emailSentResponse.Result.EmailSentCount;
            });

            response.Message = $"Total users: {response.UsersCount} Email sent: {response.EmailSentCount}.";

            return response;
        }

        public async Task<IndustryPlacementNotificationResponse> ProcessIndustryPlacementOneOutstandingUlnReminderAsync()
        {
            IndustryPlacementNotificationResponse response = new();

            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();

            if (currentAcademicYears == null || !currentAcademicYears.Any())
            {
                throw new ApplicationException($"Current Academic years are not found. Method: {nameof(ProcessIndustryPlacementOneOutstandingUlnReminderAsync)}");
            }

            var previousAcademicYear = currentAcademicYears.FirstOrDefault()?.Year - 1;

            var filteredPathways = await _tqRegistrationPathwayRepository.GetManyAsync()
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                .Where(w => w.Status == RegistrationPathwayStatus.Active &&
                            w.EndDate == null &&
                            w.AcademicYear == previousAcademicYear)
                .ToListAsync();

            var pathwayWithOnePendingIPStatus = filteredPathways
                .Where(rp => !rp.IndustryPlacements.Any())
                .GroupBy(g => g.TqProvider.TlProvider.UkPrn)
                .Where(rp => rp.Count() == 1)
                .SelectMany(rp => rp)
                .ToList();

            if (pathwayWithOnePendingIPStatus == null || !pathwayWithOnePendingIPStatus.Any())
            {
                var message = $"No entries are found. Method: {nameof(ProcessIndustryPlacementOneOutstandingUlnReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(pathwayWithOnePendingIPStatus.Select(x => x.TqProvider.TlProvider.UkPrn).ToList());

            if (providerUsers == null)
            {
                var message = $"No provider users are found. Method: {nameof(ProcessIndustryPlacementOneOutstandingUlnReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new IndustryPlacementNotificationResponse { IsSuccess = true, Message = message };
            }

            pathwayWithOnePendingIPStatus.ForEach(rp =>
            {
                Dictionary<string, dynamic> userTokens = new()
                {
                    { "ULN", rp.TqRegistrationProfile.UniqueLearnerNumber }
                };

                var dfeUsers = providerUsers.Where(providerUsers => providerUsers.Ukprn == rp.TqProvider.TlProvider.UkPrn.ToString()).ToList();

                if (dfeUsers == null || !dfeUsers.Any())
                    return;

                var emailSentResponse = SendEmailNotificationAsync(NotificationTemplateName.IndustryPlacementOneOutstandingUlnReminder.ToString(), dfeUsers, userTokens);

                response.IsSuccess = emailSentResponse.Result.IsSuccess;
                response.UsersCount += emailSentResponse.Result.UsersCount;
                response.EmailSentCount += emailSentResponse.Result.EmailSentCount;
            });

            response.Message = $"Total users: {response.UsersCount} Email sent: {response.EmailSentCount}.";

            return response;
        }

        private async Task<IndustryPlacementNotificationResponse> SendEmailNotificationAsync(string templateName, IEnumerable<DfeUsers> serviceUsers, IDictionary<string, dynamic> tokens)
        {
            var users = serviceUsers.SelectMany(u => u.Users).ToList();

            int emailSentCount = 0;
            var hasEmailSent = false;

            _logger.LogInformation($"Sending email notification for {templateName} to {users.Count()} users.");

            // TODO temp overriding email address
            users.ForEach(u => u.Email = "sajid.malik@education.gov.uk");

            foreach (var user in users)
            {
                hasEmailSent = await _notificationService.SendEmailNotificationAsync(templateName, user.Email, tokens);

                if (hasEmailSent)
                    emailSentCount++;
            }

            return new IndustryPlacementNotificationResponse
            {
                UsersCount = users.Count,
                EmailSentCount = emailSentCount,
                IsSuccess = hasEmailSent
            };
        }
    }
}
