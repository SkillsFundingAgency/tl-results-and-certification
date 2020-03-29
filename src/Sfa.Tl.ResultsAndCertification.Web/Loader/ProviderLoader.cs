using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ProviderLoader : IProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public ProviderLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the provider lookup data asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isExactMatch">if set to <c>true</c> [is exact match].</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProviderLookupData>> GetProviderLookupDataAsync(string name, bool isExactMatch)
        {
            var providers = await _internalApiClient.FindProviderAsync(name, isExactMatch);
            return _mapper.Map<IEnumerable<ProviderLookupData>>(providers);
        }

        /// <summary>
        /// Determines whether [is any provider setup completed asynchronous] [the specified ukprn].
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <returns></returns>
        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            return await _internalApiClient.IsAnyProviderSetupCompletedAsync(ukprn);
        }

        /// <summary>
        /// Gets the select provider tlevels asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<ProviderTlevelsViewModel> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var tlevelDetails = await _internalApiClient.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
            return _mapper.Map<ProviderTlevelsViewModel>(tlevelDetails);
        }

        /// <summary>
        /// Adds the provider tlevels asynchronous.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public async Task<bool> AddProviderTlevelsAsync(ProviderTlevelsViewModel viewModel)
        {
            var addViewModel = _mapper.Map<IList<ProviderTlevelDetails>>(viewModel.Tlevels.Where(x => x.IsSelected).ToList());
            return await _internalApiClient.AddProviderTlevelsAsync(addViewModel);
        }

        /// <summary>
        /// Gets the view provider tlevel view model asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<ProviderTlevelsViewModel> GetViewProviderTlevelViewModelAsync(long aoUkprn, int providerId)
        {
            var tlevelDetails = await _internalApiClient.GetProviderTlevelsAsync(aoUkprn, providerId);
            return _mapper.Map<ProviderTlevelsViewModel>(tlevelDetails);
        }

        /// <summary>
        /// Gets the tq ao provider details asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <returns></returns>
        public async Task<IList<ProviderDetailsViewModel>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var providerDetails = await _internalApiClient.GetTqAoProviderDetailsAsync(aoUkprn);
            return _mapper.Map<IList<ProviderDetailsViewModel>>(providerDetails);
        }
    }
}
