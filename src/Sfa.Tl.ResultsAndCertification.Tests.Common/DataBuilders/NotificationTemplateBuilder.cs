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
            Id = 1,
            TemplateId = new Guid("60581937-fcdd-4bcb-910a-04a136803091"),
            TemplateName = "TlevelDetailsQueried",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<NotificationTemplate> BuildList() => new List<NotificationTemplate>
        {
            new NotificationTemplate
            {
                Id = 1,
                TemplateId = new Guid("90581937-dddd-4bcb-910a-04a136803091"),
                TemplateName = "TlevelDetailsQueried",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new NotificationTemplate
            {
                Id = 2,
                TemplateId = new Guid("70581937-acdd-4bcb-910a-07a136803091"),
                TemplateName = "TlevelDetailsConfirmed",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
