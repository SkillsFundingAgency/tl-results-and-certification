using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class StatementOfAchievementLoader : IStatementOfAchievementLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public StatementOfAchievementLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _internalApiClient.FindSoaLearnerRecordAsync(providerUkprn, uln);            
        }

        public async Task<SoaLearnerRecordDetailsViewModel> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            var response = await _internalApiClient.GetSoaLearnerRecordDetailsAsync(providerUkprn, profileId);
            return _mapper.Map<SoaLearnerRecordDetailsViewModel>(response);
        }

        public async Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(long providerUkprn, SoaLearnerRecordDetailsViewModel viewModel)
        {
            var request = _mapper.Map<SoaPrintingRequest>(viewModel, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.CreateSoaPrintingRequestAsync(request);
        }
    }
}