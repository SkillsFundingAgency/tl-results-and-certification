using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class DashboardLoader : IDashboardLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;

        public DashboardLoader(IResultsAndCertificationInternalApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<DashboardViewModel> GetDashboardViewModel(ClaimsPrincipal claimsPrincipal)
        {
            LoginUserType? loggedInUserType = claimsPrincipal.GetLoggedInUserType();

            bool hasAccessToService = claimsPrincipal.HasAccessToService() && loggedInUserType.HasValue;
            if (!hasAccessToService)
            {
                return new DashboardViewModel { HasAccessToService = false };
            }

            LoginUserType loginUserType = loggedInUserType.Value;
            IEnumerable<string> bannerMessages = await GetBannerMessages(loginUserType);

            return new DashboardViewModel
            {
                HasAccessToService = true,
                LoginUserType = loginUserType,
                Banners = bannerMessages.Select(msg => new DashboardBannerModel(msg))
            };
        }

        private Task<IEnumerable<string>> GetBannerMessages(LoginUserType loginUserType)
            => loginUserType switch
            {
                LoginUserType.AwardingOrganisation => _apiClient.GetAwardingOrganisationBanners(),
                LoginUserType.TrainingProvider => _apiClient.GetProviderBanners(),
                _ => Task.FromResult(Enumerable.Empty<string>())
            };
    }
}