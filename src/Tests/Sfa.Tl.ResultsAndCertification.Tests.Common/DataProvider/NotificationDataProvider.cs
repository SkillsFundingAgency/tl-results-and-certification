using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class NotificationDataProvider
    {
        public static NotificationTemplate CreateNotificationTemplate(ResultsAndCertificationDbContext _dbContext, NotificationTemplateName notificationTemplateName = NotificationTemplateName.TlevelDetailsQueried, bool addToDbContext = true)
        {
            var notificationTemplate = new NotificationTemplateBuilder().Build(notificationTemplateName);

            if (addToDbContext)
            {
                _dbContext.Add(notificationTemplate);
            }
            return notificationTemplate;
        }

        public static NotificationTemplate CreateNotificationTemplate(ResultsAndCertificationDbContext _dbContext, NotificationTemplate notificationTemplate, bool addToDbContext = true)
        {
            if (notificationTemplate == null)
            {
                notificationTemplate = new NotificationTemplateBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(notificationTemplate);
            }
            return notificationTemplate;
        }

        public static NotificationTemplate CreateNotificationTemplate(ResultsAndCertificationDbContext _dbContext, Guid templateId, string templateName, bool addToDbContext = true)
        {
            var notificationTemplate = new NotificationTemplate
            {
                TemplateId = templateId,
                TemplateName = templateName
            };

            if (addToDbContext)
            {
                _dbContext.Add(notificationTemplate);
            }
            return notificationTemplate;
        }
    }
}
