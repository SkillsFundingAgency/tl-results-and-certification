using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class PostResultsServiceLoader : IPostResultsServiceLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public PostResultsServiceLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null)
        {
            return await _internalApiClient.FindPrsLearnerRecordAsync(aoUkprn, uln, profileId);
        }

        public async Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, RegistrationPathwayStatus.Active);

            var viewModel = _mapper.Map<T>(response, opt => opt.Items[Constants.ProfileId] = profileId);

            return viewModel;
        }

        public async Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId, ComponentType componentType)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, RegistrationPathwayStatus.Active);

            if (response == null || componentType == ComponentType.NotSpecified)
                return default;

            Specialism specialism = null;
            Assessment assessment = null;

            switch (componentType)
            {
                case ComponentType.Core:
                    assessment = response.Pathway.PathwayAssessments.FirstOrDefault(p => p.Id == assessmentId);
                    break;
                case ComponentType.Specialism:
                    specialism = response.Pathway.Specialisms.FirstOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));
                    assessment = specialism?.Assessments?.FirstOrDefault(sa => sa.Id == assessmentId);
                    break;
            }

            var hasResult = assessment?.Result?.Id > 0;

            if (assessment == null || !hasResult)
                return default;

            if (typeof(T) == typeof(PrsRommGradeChangeViewModel) || typeof(T) == typeof(PrsAppealGradeChangeViewModel))
            {
                var grades = await _internalApiClient.GetLookupDataAsync(componentType == ComponentType.Core ? LookupCategory.PathwayComponentGrade : LookupCategory.SpecialismComponentGrade);
                return _mapper.Map<T>(response, opt => { opt.Items["assessment"] = assessment; opt.Items["specialism"] = specialism; opt.Items["grades"] = grades; });
            }
            else
            {
                return _mapper.Map<T>(response, opt =>
                {
                    opt.Items["specialism"] = specialism;
                    opt.Items["assessment"] = assessment;
                });
            }
        }

        public async Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessementId)
        {
            var prsLearnerDetails = await _internalApiClient.GetPrsLearnerDetailsAsync(aoUkprn, profileId, assessementId);
            return _mapper.Map<T>(prsLearnerDetails);
        }

        public async Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeKnownViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsRommActivityAsync(long aoUkprn, PrsRommCheckAndSubmitViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);

            // Assign new grade lookup id
            var lookupCategory = model.ComponentType == ComponentType.Core ? LookupCategory.PathwayComponentGrade : LookupCategory.SpecialismComponentGrade;
            var grades = await _internalApiClient.GetLookupDataAsync(lookupCategory);
            var newGrade = grades.FirstOrDefault(x => x.Value.Equals(model.NewGrade, StringComparison.InvariantCultureIgnoreCase));
            if (newGrade == null)
                return false;
            request.ResultLookupId = newGrade.Id;

            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeKnownViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAppealCheckAndSubmitViewModel model)
        {
            var request = _mapper.Map<PrsActivityRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);

            // Assign new grade lookup id
            var lookupCategory = model.ComponentType == ComponentType.Core ? LookupCategory.PathwayComponentGrade : LookupCategory.SpecialismComponentGrade;
            var grades = await _internalApiClient.GetLookupDataAsync(lookupCategory);
            var newGrade = grades.FirstOrDefault(x => x.Value.Equals(model.NewGrade, StringComparison.InvariantCultureIgnoreCase));
            if (newGrade == null)
                return false;
            request.ResultLookupId = newGrade.Id;

            return await _internalApiClient.PrsActivityAsync(request);
        }

        public async Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequestViewModel model)
        {
            var request = _mapper.Map<PrsGradeChangeRequest>(model);
            return await _internalApiClient.PrsGradeChangeRequestAsync(request);
        }

        public T TransformLearnerDetailsTo<T>(FindPrsLearnerRecord prsLearnerRecord)
        {
            return _mapper.Map<T>(prsLearnerRecord);
        }
    }
}