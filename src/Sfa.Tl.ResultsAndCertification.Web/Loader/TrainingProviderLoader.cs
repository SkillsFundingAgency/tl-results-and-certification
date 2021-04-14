using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

        public async Task<UpdateLearnerRecordResponseViewModel> ProcessIndustryPlacementQuestionUpdateAsync(long providerUkprn, UpdateIndustryPlacementQuestionViewModel viewModel)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, viewModel.ProfileId, viewModel.RegistrationPathwayId);

            if (response == null || !response.IsLearnerRecordAdded) return null;

            if (response.IndustryPlacementStatus == viewModel.IndustryPlacementStatus)
            {
                return new UpdateLearnerRecordResponseViewModel { IsModified = false };
            }

            var request = _mapper.Map<UpdateLearnerRecordRequest>(viewModel, opt => { opt.Items["providerUkprn"] = providerUkprn; opt.Items["uln"] = response.Uln; });
            var isSuccess = await _internalApiClient.UpdateLearnerRecordAsync(request);
            return new UpdateLearnerRecordResponseViewModel { ProfileId = response.ProfileId, Uln = response.Uln, Name = response.Name, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<UpdateLearnerRecordResponseViewModel> ProcessEnglishAndMathsQuestionUpdateAsync(long providerUkprn, UpdateEnglishAndMathsQuestionViewModel viewModel)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, viewModel.ProfileId);

            if (response == null || !response.IsLearnerRecordAdded || response.HasLrsEnglishAndMaths) return null;

            var englishAndMathsStatus = GetEnglishAndMathsStatus(response);

            if (englishAndMathsStatus == viewModel.EnglishAndMathsStatus)
            {
                return new UpdateLearnerRecordResponseViewModel { IsModified = false };
            }
                        
            viewModel.HasLrsEnglishAndMaths = response.HasLrsEnglishAndMaths;
            var request = _mapper.Map<UpdateLearnerRecordRequest>(viewModel, opt => { opt.Items["providerUkprn"] = providerUkprn; opt.Items["uln"] = response.Uln; });
            var isSuccess = await _internalApiClient.UpdateLearnerRecordAsync(request);
            return new UpdateLearnerRecordResponseViewModel { ProfileId = response.ProfileId, Uln = response.Uln, Name = response.Name, IsModified = true, IsSuccess = isSuccess };
        }

        private EnglishAndMathsStatus? GetEnglishAndMathsStatus(LearnerRecordDetails model)
        {
            if (model.HasLrsEnglishAndMaths)
                return null;

            if (model.IsEnglishAndMathsAchieved && model.IsSendLearner == true)
            {
                return EnglishAndMathsStatus.AchievedWithSend;
            }
            else if (model.IsEnglishAndMathsAchieved)
            {
                return EnglishAndMathsStatus.Achieved;
            }
            else
            {
                return !model.IsEnglishAndMathsAchieved ? (EnglishAndMathsStatus?)EnglishAndMathsStatus.NotAchieved : null;
            }
        }
    }
}