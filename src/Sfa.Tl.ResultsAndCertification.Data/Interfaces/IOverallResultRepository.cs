using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IOverallResultRepository : IRepository<OverallResult>
    {
        Task<IList<OverallResult>> GetOverallResults(long providerUkprn, DateTime resultPublishDate);
    }
}