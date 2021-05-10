using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ProviderAddressLoader : IProviderAddressLoader
    {
        private readonly IOrdnanceSurveyApiClient _ordnanceSurveyApiClient;
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderAddressLoader> _logger;

        public ProviderAddressLoader(IOrdnanceSurveyApiClient ordnanceSurveyApiClient, IMapper mapper, ILogger<ProviderAddressLoader> logger)
        {
            _ordnanceSurveyApiClient = ordnanceSurveyApiClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddAddressSelectViewModel> GetAddressesByPostcodeAsync(string postcode)
        {
            try
            {
                var response = await _ordnanceSurveyApiClient.GetAddressesByPostcodeAsync(postcode);
                return _mapper.Map<AddAddressSelectViewModel>(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvent.UnableToGetAddressFromOrdnanceSurvey, ex, $"Unable to get addresses by postcode from Ordnance Survey. Postcode={postcode}, ErrorMessage={ex.Message}");
                return new AddAddressSelectViewModel();
            }
        }

        public async Task<AddressViewModel> GetAddressByUprnAsync(long uprn)
        {
            try
            {
                var response = await _ordnanceSurveyApiClient.GetAddressByUprnAsync(uprn);
                return _mapper.Map<AddressViewModel>(response?.AddressResult?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvent.UnableToGetAddressFromOrdnanceSurvey, ex, $"Unable to get address by uprn from Ordnance Survey. Uprn={uprn}, ErrorMessage={ex.Message}");
                return null;
            }
        }
    }
}