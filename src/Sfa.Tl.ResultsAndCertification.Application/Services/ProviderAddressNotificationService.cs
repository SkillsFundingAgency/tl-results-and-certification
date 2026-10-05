using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class ProviderAddressNotificationService : IProviderAddressNotificationService
    {
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IDfeSignInApiClient _dfeSignInApiClient;
        private readonly INotificationService _notificationService;

        private readonly ILogger _logger;

        public ProviderAddressNotificationService(
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IDfeSignInApiClient dfeSignInApiClient,
            INotificationService notificationService,
            ILogger<ProviderAddressNotificationService> logger)
        {
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _dfeSignInApiClient = dfeSignInApiClient;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<ProviderAddressNotificationResponse> ProcessProviderAddressValidationReminderAsync(int academicYearToProcess)
        {
            if (academicYearToProcess <= 0)
            {
                throw new ApplicationException($"Academic year to process cannot be 0. {nameof(ProcessProviderAddressValidationReminderAsync)}");
            }

            var currentAcademicYearProviders = await _tqRegistrationPathwayRepository
                .GetManyAsync(rp => rp.AcademicYear == academicYearToProcess && rp.Status == RegistrationPathwayStatus.Active)
                .Where(p => p.TqProvider.TlProvider.IsActive && p.TqProvider.TlProvider.TlProviderAddresses.Any(pa => pa.IsActive))
                .Select(p => p.TqProvider.TlProvider)
                .ToListAsync();

            var providerWithAddress = currentAcademicYearProviders
                .DistinctBy(p => p.UkPrn)
                .Select(p => p.UkPrn)
                .ToList();

            if (currentAcademicYearProviders == null || !currentAcademicYearProviders.Any())
            {
                throw new ApplicationException($"There are no active providers. Method: {nameof(ProcessProviderAddressValidationReminderAsync)}");
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(providerWithAddress);

            if (providerUsers == null || !providerUsers.Any())
            {
                var message = $"No provider users are found. Method: {nameof(ProcessProviderAddressValidationReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new ProviderAddressNotificationResponse { IsSuccess = true, Message = message };
            }

            Dictionary<string, dynamic> userTokens = new()
            {
                { "reference_number", 000000 }
            };

            var response = await SendEmailNotificationAsync(NotificationTemplateName.ProviderAddressValidateReminder.ToString(), providerUsers, userTokens);

            response.Message = $"Total users: {response.UsersCount} Email sent: {response.EmailSentCount}.";

            return response;
        }

        public async Task<ProviderAddressNotificationResponse> ProcessProviderAddressMissingReminderAsync(int academicYearToProcess)
        {
            if (academicYearToProcess <= 0)
            {
                throw new ApplicationException($"Academic year to process cannot be 0. {nameof(ProcessProviderAddressMissingReminderAsync)}");
            }

            var currentAcademicYearProviders = await _tqRegistrationPathwayRepository
                .GetManyAsync(rp => rp.AcademicYear == academicYearToProcess && rp.Status == RegistrationPathwayStatus.Active)
                .Where(p => p.TqProvider.TlProvider.IsActive && !p.TqProvider.TlProvider.TlProviderAddresses.Any())
                .Select(p => p.TqProvider.TlProvider)
                .ToListAsync();

            var providerWithoutAddress = currentAcademicYearProviders
                .DistinctBy(p => p.UkPrn)
                .Select(p => p.UkPrn)
                .ToList();

            if (currentAcademicYearProviders == null || !currentAcademicYearProviders.Any())
            {
                throw new ApplicationException($"There are no active providers. Method: {nameof(ProcessProviderAddressMissingReminderAsync)}");
            }

            var providerUsers = await _dfeSignInApiClient.GetDfeUsersAllProviders(providerWithoutAddress);

            if (providerUsers == null || !providerUsers.Any())
            {
                var message = $"No provider users are found. Method: {nameof(ProcessProviderAddressMissingReminderAsync)}()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new ProviderAddressNotificationResponse { IsSuccess = true, Message = message };
            }

            Dictionary<string, dynamic> userTokens = new()
            {
                { "reference_number", 000000 }
            };

            var response = await SendEmailNotificationAsync(NotificationTemplateName.ProviderAddressMissingReminder.ToString(), providerUsers, userTokens);

            response.Message = $"Total users: {response.UsersCount} Email sent: {response.EmailSentCount}.";

            return response;
        }

        private async Task<ProviderAddressNotificationResponse> SendEmailNotificationAsync(string templateName, IEnumerable<DfeUsers> serviceUsers, IDictionary<string, dynamic> tokens)
        {
            var users = serviceUsers.SelectMany(u => u.Users).ToList();

            users = new List<ServiceUser>() { new ServiceUser() { Email = "sajid.malik@education.gov.uk" } };

            int emailSentCount = 0;
            var hasEmailSent = false;

            _logger.LogInformation($"Sending email notification for {templateName} to {users.Count()} users.");

            foreach (var user in users)
            {
                hasEmailSent = await _notificationService.SendEmailNotificationAsync(templateName, user.Email, tokens);

                if (hasEmailSent)
                    emailSentCount++;
            }

            return new ProviderAddressNotificationResponse
            {
                UsersCount = users.Count,
                EmailSentCount = emailSentCount,
                IsSuccess = hasEmailSent
            };
        }
    }
}