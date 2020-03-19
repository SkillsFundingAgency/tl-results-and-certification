using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IAsyncNotificationClient _notificationClient;
        private readonly IRepository<NotificationTemplate> _notificationTemplateRepository;

        public NotificationService(IRepository<NotificationTemplate> notificationTemplateRepository, IAsyncNotificationClient notificationClient, ILogger<NotificationService> logger)
        {
            _notificationTemplateRepository = notificationTemplateRepository;
            _notificationClient = notificationClient;
            _logger = logger;
        }

        public async Task<bool> SendEmailNotificationAsync(string templateName, string toAddress, IDictionary<string, dynamic> tokens)
        {
            var hasEmailSent = false;
            var notificationTemplate = await _notificationTemplateRepository.GetFirstOrDefaultAsync(t => t.TemplateName == templateName);
            if (notificationTemplate == null)
            {
                _logger.LogWarning($"Notification email template {templateName} not found");
                return hasEmailSent;
            }

            try
            {
                var personalisationTokens = tokens.ToDictionary(t => t.Key, t => t.Value);
                await _notificationClient.SendEmailAsync(emailAddress: toAddress, templateId: notificationTemplate.TemplateId.ToString(), personalisation: personalisationTokens);
                hasEmailSent = true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification email, templateName: {notificationTemplate.TemplateName}, templateId: {notificationTemplate.TemplateId} to {toAddress}");
            }
            return hasEmailSent;
        }
    }
}
