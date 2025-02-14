using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminDownloadLearnerResultsLoader : IAdminDownloadLearnerResultsLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;

        public AdminDownloadLearnerResultsLoader(IResultsAndCertificationInternalApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AdminDownloadLearnerResultsByProviderViewModel> GetDownloadLearnerResultsByProviderViewModel(int providerId)
        {
            GetProviderResponse response = await _apiClient.GetProviderAsync(providerId);

            if (response == null)
            {
                return null;
            }

            return new AdminDownloadLearnerResultsByProviderViewModel
            {
                ProviderUkprn = response.UkPrn,
                ProviderName = response.DisplayName
            };
        }
    }
}