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

        public async Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
            return _mapper.Map<T>(response);
        }

        public async Task<AddLearnerRecordResponse> AddLearnerRecordAsync(long providerUkprn, AddLearnerRecordViewModel viewModel)
        {
            var learnerRecordModel = _mapper.Map<AddLearnerRecordRequest>(viewModel, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.AddLearnerRecordAsync(learnerRecordModel);
        }

        public async Task<UpdateLearnerRecordResponse> ProcessIndustryPlacementQuestionUpdateAsync(long providerUkprn, UpdateIndustryPlacementQuestionViewModel viewModel)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, viewModel.ProfileId, viewModel.RegistrationPathwayId);

            if (response == null || !response.IsLearnerRecordAdded) return null;

            if (response.IndustryPlacementStatus == viewModel.IndustryPlacementStatus)
            {
                return new UpdateLearnerRecordResponse { IsModified = false };
            }

            var request = _mapper.Map<UpdateLearnerRecordRequest>(viewModel, opt => opt.Items["providerUkprn"] = providerUkprn);           
            var isSuccess = await _internalApiClient.UpdateLearnerRecordAsync(request);
            return new UpdateLearnerRecordResponse { ProfileId = response.ProfileId, Uln = response.Uln, Name = response.Name, IsModified = true, IsSuccess = isSuccess };
        }
    }
}