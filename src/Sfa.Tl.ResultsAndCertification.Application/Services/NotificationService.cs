using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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
        private readonly ILogger _logger;
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
                _logger.LogWarning(LogEvent.EmailTemplateNotFound, $"Notification email template {templateName} not found");
                return hasEmailSent;
            }

            try
            {
                var personalisationTokens = tokens.ToDictionary(t => t.Key, t => t.Value);
                await _notificationClient.SendEmailAsync(emailAddress: toAddress, templateId: notificationTemplate.TemplateId.ToString(), personalisation: personalisationTokens);
                hasEmailSent = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvent.EmailSendFailed, ex, $"Error sending notification email, templateName: {notificationTemplate.TemplateName}, templateId: {notificationTemplate.TemplateId} to {toAddress}");
            }
            return hasEmailSent;
        }

        public async Task<bool> SendEmailNotificationAsync(string templateName, List<string> recipients, IDictionary<string, dynamic> tokens)
        {
            if (recipients == null || !recipients.Any())
            {
                _logger.LogWarning(LogEvent.EmailRecipientsNotFound, $"No recipients found for notification email template {templateName}");
                return false;
            }

            var hasEmailSent = false;
            var notificationTemplate = await _notificationTemplateRepository.GetFirstOrDefaultAsync(t => t.TemplateName == templateName);
            if (notificationTemplate == null)
            {
                _logger.LogWarning(LogEvent.EmailTemplateNotFound, $"Notification email template {templateName} not found");
                return hasEmailSent;
            }

            var personalisationTokens = tokens.ToDictionary(t => t.Key, t => t.Value);

            List<Task> emailTasks = new();

            foreach (var recipient in recipients)
            {
                try
                {
                    emailTasks.Add(_notificationClient.SendEmailAsync(emailAddress: recipient, templateId: notificationTemplate.TemplateId.ToString(), personalisation: personalisationTokens));
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogEvent.EmailSendFailed, ex, $"Error sending notification email, templateName: {notificationTemplate.TemplateName}, templateId: {notificationTemplate.TemplateId} to {recipient}");
                }
            }
            await Task.WhenAll(emailTasks);
            hasEmailSent = true;

            return hasEmailSent;
        }
    }
}
