using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IProviderRegistrationsRepository
    {
        Task<IList<int>> GetAvailableStartYearsAsync(Func<DateTime> getToday);
        Task<IList<TqRegistrationPathway>> GetRegistrationsAsync(long providerUkprn, int startYear);
    }
}