using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public class NotificationTemplate : BaseEntity
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }        
    }
}
