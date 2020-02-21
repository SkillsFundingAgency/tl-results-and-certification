using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class TlevelLoader : ITlevelLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public TlevelLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<YourTLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<YourTLevelDetailsViewModel>(tLevelPathwayInfo);
        }

        public async Task<IEnumerable<YourTlevelsViewModel>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<IEnumerable<YourTlevelsViewModel>>(tLevels);
        }

        public async Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tLevels = await GetAllTlevelsByUkprnAsync(ukprn);
            var tLevelsForReview = tLevels?.Where(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation);
            return tLevelsForReview;
        }
    }
}
