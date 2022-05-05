using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class IndustryPlacementLoader : IIndustryPlacementLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public IndustryPlacementLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null)
        {
            return await _internalApiClient.GetIpLookupDataAsync(ipLookupType, pathwayId);
        }

        public async Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
            return _mapper.Map<T>(response);
        }

        public async Task<T> GetIpLookupDataAsync<T>(IpLookupType ipLookupType, string learnerName = null, int? pathwayId = null, bool showOption = false)
        {
            var lookupData = await _internalApiClient.GetIpLookupDataAsync(ipLookupType, pathwayId);

            if (lookupData == null)
                return default;

            lookupData = lookupData.Where(lkp => lkp.ShowOption == showOption || lkp.ShowOption == null).ToList();

            return _mapper.Map<T>(lookupData, opt => opt.Items["learnerName"] = learnerName);
        }

        public async Task<T> TransformIpCompletionDetailsTo<T>(IpCompletionViewModel model)
        {
            return await Task.FromResult(_mapper.Map<T>(model));
        }

        public async Task<IList<IpLookupDataViewModel>> GetSpecialConsiderationReasonsListAsync(int academicYear)
        {
            var scReasons = await GetIpLookupDataAsync(IpLookupType.SpecialConsideration);
            return _mapper.Map<IList<IpLookupDataViewModel>>(scReasons.Where(x => academicYear >= x.StartDate.Year && (x.EndDate == null || academicYear <= x.EndDate.Value.Year)));
        }

        public async Task<IList<IpLookupDataViewModel>> GetTemporaryFlexibilitiesAsync(int pathwayId, int academicYear, bool showOption = false)
        {
            var tempFlexibilities = await GetIpLookupDataAsync(IpLookupType.TemporaryFlexibility, pathwayId);
            return _mapper.Map<IList<IpLookupDataViewModel>>(tempFlexibilities?.Where(x => academicYear >= x.StartDate.Year && (x.EndDate == null || academicYear <= x.EndDate.Value.Year) && (x.ShowOption == showOption || x.ShowOption == null)));
        }

        public async Task<IpTempFlexNavigation> GetTempFlexNavigationAsync(int pathwayId, int academicYear)
        {
            return await _internalApiClient.GetTempFlexNavigationAsync(pathwayId, academicYear);
        }
    }
}