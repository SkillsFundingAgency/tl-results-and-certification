using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IProviderRegistrationsService
    {
        Task<IList<int>> GetAvailableStartYearsAsync(Func<DateTime> getToday);
        Task<DataExportResponse> GetRegistrationsAsync(long providerUkprn, int startYear, string requestedBy, Func<Guid> newGuid);
    }
}