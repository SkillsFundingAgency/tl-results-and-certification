using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
                NotificationTemplateName.TlevelDetailsQueried => new NotificationTemplate
                {
                    TemplateId = new Guid("60581937-fcdd-4bcb-910a-04a136803091"),
                    TemplateName = templateName.ToString(),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                NotificationTemplateName.EnglishAndMathsLrsDataQueried => new NotificationTemplate
                {
                    TemplateId = new Guid("a1b21a18-8555-45b8-9739-f18a902282dc"),
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
                NotificationTemplateName.GradeChangeRequestTechnicalTeamNotification => new NotificationTemplate
                {
                    TemplateId = new Guid("11b21a18-8555-45b8-9739-f18a90228521"),
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
                TemplateName = NotificationTemplateName.TlevelDetailsQueried.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                TemplateId = new Guid("a1b21a18-8555-45b8-9739-f18a902282dc"),
                TemplateName = NotificationTemplateName.EnglishAndMathsLrsDataQueried.ToString(),
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
                TemplateId = new Guid("11b21a18-8555-45b8-9739-f18a90228521"),
                TemplateName = NotificationTemplateName.GradeChangeRequestTechnicalTeamNotification.ToString(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
