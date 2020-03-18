using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ProviderLoader : IProviderLoader
    {
        public Task<IEnumerable<object>> GetAllProvidersByUkprnAsync(long v)
        {
            throw new NotImplementedException();
        }
    }
}
