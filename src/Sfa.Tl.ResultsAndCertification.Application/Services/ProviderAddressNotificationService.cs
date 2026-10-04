using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ProviderAddressNotificationService : IProviderAddressNotificationService
    {
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<NotificationTemplate> _notificationTemplateRepository;
        private readonly IRepository<TlProvider> _tlProviderRepository;
        private readonly IDfeSignInApiClient _dfeSignInApiClient;
        private readonly INotificationService _notificationService;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly ICommonRepository _commonRepository;

        private readonly ILogger _logger;

        public ProviderAddressNotificationService(
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<NotificationTemplate> notificationTemplateRepository,
            IRepository<TlProvider> tlProviderRepository,
            ICommonRepository commonRepository,
            IDfeSignInApiClient dfeSignInApiClient,
            INotificationService notificationService,
            IAsyncNotificationClient notificationClient,
            ILogger<ProviderAddressNotificationService> logger)
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

        public Task<ProviderAddressNotificationResponse> ProcessProviderAddressValidateReminderAsync()
        {
            throw new NotImplementedException();
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