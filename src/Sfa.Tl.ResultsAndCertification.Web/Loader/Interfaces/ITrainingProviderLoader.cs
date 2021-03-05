﻿using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITrainingProviderLoader
    {
        Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln);
    }
}
