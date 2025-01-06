using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IDashboardBannerService
    {
        Task<IEnumerable<string>> GetAwardingOrganisationBanners(Func<DateTime> getToday);
        Task<IEnumerable<string>> GetProviderBanners(Func<DateTime> getToday);
    }
}