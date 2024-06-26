﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class NotificationTemplateBuilder
    {
        public NotificationTemplate Build() => new NotificationTemplate
        {
            TemplateId = new Guid("60581937-fcdd-4bcb-910a-04a136803091"),
            TemplateName = "TlevelDetailsQueried",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public NotificationTemplate Build(NotificationTemplateName templateName)
        {
            return templateName switch
            {
                NotificationTemplateName.TlevelDetailsQueriedTechnicalTeamNotification => new NotificationTemplate
                {
                    TemplateId = new Guid("60581937-fcdd-4bcb-910a-04a136803091"),
                    TemplateName = templateName.ToString(),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                NotificationTemplateName.GradeChangeRequestUserNotification => new NotificationTemplate
                {
                    TemplateId = new Guid("91b21a18-8555-45b8-9739-f18a90228211"),
                    TemplateName = templateName.ToString(),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                _ => null,
            };
        }

        public IList<NotificationTemplate> BuildList() => new List<NotificationTemplate>
        {
            new NotificationTemplate
            {
                TemplateId = new Guid("90581937-dddd-4bcb-910a-04a136803091"),
                TemplateName = NotificationTemplateName.TlevelDetailsQueriedTechnicalTeamNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("7F23DD3A-EEE4-4248-9D28-E33FD8487295"),
                TemplateName = NotificationTemplateName.TlevelDetailsQueriedUserNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("91b21a18-8555-45b8-9739-f18a90228211"),
                TemplateName = NotificationTemplateName.GradeChangeRequestUserNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("61a21a18-8555-45b8-9739-f18a90229911"),
                TemplateName = NotificationTemplateName.AppealGradeAfterDeadlineRequestUserNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("77b21a18-8555-45b8-9739-f18a90227721"),
                TemplateName = NotificationTemplateName.AppealGradeAfterDeadlineRequestTechnicalTeamNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("daa342c6-8f92-4695-80ee-f251a7844449"),
                TemplateName = NotificationTemplateName.GradeChangeRequestTechnicalTeamNotificationCoreComponent.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("559a3508-432d-4dd2-ac87-e7e15c81bcf3"),
                TemplateName = NotificationTemplateName.GradeChangeRequestTechnicalTeamNotificationSpecialism.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
