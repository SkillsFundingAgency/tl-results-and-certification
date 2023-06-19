using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IIndustryPlacementRepository
    {
        public Task<IList<IndustryPlacement>> ExtractIndustryPlacementAsync();
    }
}
