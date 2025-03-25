using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IOverallResultRepository : IRepository<OverallResult>
    {
        Task<IList<OverallResult>> GetOverallResults(long providerUkprn, int resultCalculationYear, DateTime resultPublishDate, DateTime today);

        Task<OverallResult> GetLearnerOverallResults(long providerUkprn, long profileId);
    }
}