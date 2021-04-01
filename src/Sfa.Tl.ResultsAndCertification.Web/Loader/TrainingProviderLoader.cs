using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class TrainingProviderLoader : ITrainingProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public TrainingProviderLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _internalApiClient.FindLearnerRecordAsync(providerUkprn, uln);
        }

        public async Task<LearnerRecordDetailsViewModel> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId);
            return _mapper.Map<LearnerRecordDetailsViewModel>(response);
        }

        public async Task<AddLearnerRecordResponse> AddLearnerRecordAsync(long ukprn, AddLearnerRecordViewModel viewModel)
        {
            var learnerRecordModel = _mapper.Map<AddLearnerRecordRequest>(viewModel, opt => opt.Items["Ukprn"] = ukprn);
            return await _internalApiClient.AddLearnerRecordAsync(learnerRecordModel);
        }
    }
}