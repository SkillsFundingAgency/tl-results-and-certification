﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IProviderController
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch);
        Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<IActionResult> AddProviderTlevelsAsync(IList<ProviderTlevel> model);
        Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn);
        Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId);
        Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId);
        Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId);
        Task<IList<PathwayDetails>> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn);
    }
}
