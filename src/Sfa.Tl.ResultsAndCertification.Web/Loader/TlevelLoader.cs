using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

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

        public async Task<TLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<TLevelDetailsViewModel>(tLevelPathwayInfo);
        }

        public async Task<IEnumerable<YourTlevelsViewModel>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<IEnumerable<YourTlevelsViewModel>>(tLevels);
        }

        public async Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            var tLevels = await _internalApiClient.GetTlevelsByStatusIdAsync(ukprn, statusId);
            return _mapper.Map<IEnumerable<YourTlevelsViewModel>>(tLevels);
        }

        public async Task<SelectToReviewPageViewModel> GetTlevelsToReviewByUkprnAsync(long ukprn)
        {
            var tLevels = await _internalApiClient.GetAllTlevelsByUkprnAsync(ukprn);
            return _mapper.Map<SelectToReviewPageViewModel>(tLevels);
        }

        public async Task<VerifyTlevelViewModel> GetVerifyTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tLevelPathwayInfo = await _internalApiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return _mapper.Map<VerifyTlevelViewModel>(tLevelPathwayInfo);
        }

        public async Task<bool?> ConfirmTlevelAsync(VerifyTlevelViewModel viewModel)
        {
            var confirmModel = _mapper.Map<ConfirmTlevelDetails>(viewModel);
            return await _internalApiClient.ConfirmTlevelAsync(confirmModel);
        }
    }
}
