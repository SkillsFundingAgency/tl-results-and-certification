using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class PostResultsServiceLoader : IPostResultsServiceLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<PostResultsServiceLoader> _logger;

        public PostResultsServiceLoader(IResultsAndCertificationInternalApiClient internalApiClient, ILogger<PostResultsServiceLoader> logger, IBlobStorageService blobStorageService, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
            _blobStorageService = blobStorageService;
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
                var grades = await GetGradesApplicable(componentType);
                return _mapper.Map<T>(response, opt =>
                {
                    opt.Items["assessment"] = assessment;
                    opt.Items["specialism"] = specialism;
                    opt.Items["grades"] = grades;
                });
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

        private async Task<IList<LookupData>> GetGradesApplicable(ComponentType componentType)
        {
            var grades = await _internalApiClient.GetLookupDataAsync(componentType == ComponentType.Core ? LookupCategory.PathwayComponentGrade : LookupCategory.SpecialismComponentGrade);

            return grades.Where(x => (componentType == ComponentType.Core && !x.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)) ||
                                     (componentType == ComponentType.Specialism && !x.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase))).ToList();
        }

        public async Task<UploadRommsResponseViewModel> ProcessBulkRommsAsync(UploadRommsRequestViewModel viewModel)
        {
            var bulkRommsRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkRommsRequest.DocumentType.ToString(),
                    BlobFileName = bulkRommsRequest.BlobFileName,
                    SourceFilePath = $"{bulkRommsRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkRommsRequest.PerformedBy
                });
            }

            var bulkRommsResponse = await _internalApiClient.ProcessBulkRommsAsync(bulkRommsRequest);
            return _mapper.Map<UploadRommsResponseViewModel>(bulkRommsResponse);
        }

        public async Task<Stream> GetRommValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var tlevelDetails = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (tlevelDetails != null && tlevelDetails.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Romms.ToString(),
                    BlobFileName = tlevelDetails.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkProcessStatus.ValidationErrors}"
                });

                if (fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download withdrawal validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.Withdrawals}, BlobFileName = {tlevelDetails.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkProcessStatus.ValidationErrors})";
                    _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
                }
                return fileStream;
            }
            else
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No DocumentUploadHistoryDetails found or the request is not valid. Method: GetDocumentUploadHistoryDetailsAsync(AoUkprn: {aoUkprn}, BlobUniqueReference = {blobUniqueReference})");
                return null;
            }
        }
    }
}