using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeStartYearRequest
    {
        public int PathwayId { get; set; }        
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public int AcademicYearTo { get; set; }
        public string DisplayAcademicYear { get; set; }
        public string ContactName { get; set; }       
        public string RequestDate { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }       
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public string CreatedBy { get; set; }

       public ChangeStartYearDetails changeStartYearDetails { get; set; }
    }
}
