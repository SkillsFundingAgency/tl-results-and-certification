using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
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

        public ProviderAddressLoader(IOrdnanceSurveyApiClient ordnanceSurveyApiClient, IMapper mapper)
        {
            _ordnanceSurveyApiClient = ordnanceSurveyApiClient;
            _mapper = mapper;
        }

        public async Task<AddAddressSelectViewModel> GetAddressesByPostcodeAsync(string postcode)
        {
            try
            {
                var response = await _ordnanceSurveyApiClient.GetAddressesByPostcode(postcode);
                return _mapper.Map<AddAddressSelectViewModel>(response);
            }
            catch
            {
                return new AddAddressSelectViewModel();
            }
        }

        public async Task<AddressViewModel> GetAddressByUprn(long uprn)
        {
            try
            {
                var response = await _ordnanceSurveyApiClient.GetAddressByUprn(uprn);
                return _mapper.Map<AddressViewModel>(response?.AddressResult?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}