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

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln, bool? evaluateSendConfirmation = false)
        {
            return await _internalApiClient.FindLearnerRecordAsync(providerUkprn, uln, evaluateSendConfirmation);
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

        public async Task<UpdateLearnerRecordResponseViewModel> ProcessIndustryPlacementQuestionUpdateAsync(long providerUkprn, UpdateIndustryPlacementQuestionViewModel viewModel)
        {
            var learnerRecordDetails = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, viewModel.ProfileId, viewModel.RegistrationPathwayId);

            if (learnerRecordDetails == null || !learnerRecordDetails.IsLearnerRecordAdded) return null;

            if (learnerRecordDetails.IndustryPlacementStatus == viewModel.IndustryPlacementStatus)
            {
                return new UpdateLearnerRecordResponseViewModel { IsModified = false };
            }

            var request = _mapper.Map<UpdateLearnerRecordRequest>(viewModel, opt => { opt.Items["providerUkprn"] = providerUkprn; opt.Items["uln"] = learnerRecordDetails.Uln; });
            var isSuccess = await _internalApiClient.UpdateLearnerRecordAsync(request);
            return new UpdateLearnerRecordResponseViewModel { ProfileId = learnerRecordDetails.ProfileId, Uln = learnerRecordDetails.Uln, Name = learnerRecordDetails.Name, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddMathsStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }
        public async Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddEnglishStatusViewModel model)
        {
            var learnerSubjectRequest = _mapper.Map<UpdateLearnerSubjectRequest>(model, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.UpdateLearnerSubjectAsync(learnerSubjectRequest);
        }
    }
}