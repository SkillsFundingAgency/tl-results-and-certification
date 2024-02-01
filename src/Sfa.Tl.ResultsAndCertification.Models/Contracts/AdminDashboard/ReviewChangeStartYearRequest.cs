using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeStartYearRequest :ReviewChangeRequest
    {
        public int RegistrationPathwayId { get; set; }        
        public long Uln { get; set; }       
        public int AcademicYear { get; set; }
        public int AcademicYearTo { get; set; }
        public string DisplayAcademicYear { get; set; }
        public string ContactName { get; set; }       
        public string RequestDate { get; set; }       
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public string CreatedBy { get; set; }

      public ChangeStartYearDetails ChangeStartYearDetails { get; set; }

      public override ChangeType ChangeType { get; set; } = ChangeType.StartYear;
    }
}
