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
      public ChangeStartYearDetails ChangeStartYearDetails { get; set; }
    }
}
