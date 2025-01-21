﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class RegistrationLoader : IRegistrationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrationLoader> _logger;

        public RegistrationLoader(IMapper mapper, ILogger<RegistrationLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _logger = logger;
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadRegistrationsResponseViewModel> ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel viewModel)
        {
            var bulkRegistrationRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkRegistrationRequest.DocumentType.ToString(),
                    BlobFileName = bulkRegistrationRequest.BlobFileName,
                    SourceFilePath = $"{bulkRegistrationRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkRegistrationRequest.PerformedBy
                });
            }

            var bulkRegistrationResponse = await _internalApiClient.ProcessBulkRegistrationsAsync(bulkRegistrationRequest);
            return _mapper.Map<UploadRegistrationsResponseViewModel>(bulkRegistrationResponse);
        }

        public async Task<UploadWithdrawalsResponseViewModel> ProcessBulkWithdrawalsAsync(UploadWithdrawalsRequestViewModel viewModel)
        {
            var bulkWithdrawalsRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkWithdrawalsRequest.DocumentType.ToString(),
                    BlobFileName = bulkWithdrawalsRequest.BlobFileName,
                    SourceFilePath = $"{bulkWithdrawalsRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkWithdrawalsRequest.PerformedBy
                });
            }

            var bulkWithdrawalsResponse = await _internalApiClient.ProcessBulkWithdrawalsAsync(bulkWithdrawalsRequest);
            return _mapper.Map<UploadWithdrawalsResponseViewModel>(bulkWithdrawalsResponse);
        }

        public async Task<Stream> GetRegistrationValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var tlevelDetails = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (tlevelDetails != null && tlevelDetails.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Registrations.ToString(),
                    BlobFileName = tlevelDetails.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkProcessStatus.ValidationErrors}"
                });

                if (fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download registration validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.Registrations}, BlobFileName = {tlevelDetails.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkProcessStatus.ValidationErrors})";
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

        public async Task<Stream> GetWithdrawalValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var tlevelDetails = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (tlevelDetails != null && tlevelDetails.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Withdrawals.ToString(),
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

        public async Task<SelectProviderViewModel> GetRegisteredTqAoProviderDetailsAsync(long aoUkprn)
        {
            var providerDetails = await _internalApiClient.GetTqAoProviderDetailsAsync(aoUkprn);
            return _mapper.Map<SelectProviderViewModel>(providerDetails);
        }

        public async Task<SelectCoreViewModel> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn)
        {
            var providerPathways = await _internalApiClient.GetRegisteredProviderPathwayDetailsAsync(aoUkprn, providerUkprn);
            return _mapper.Map<SelectCoreViewModel>(providerPathways);
        }

        public async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            var pathwaySpecialisms = await _internalApiClient.GetPathwaySpecialismsByPathwayLarIdAsync(aoUkprn, pathwayLarId);
            var pathwaySpecialismsViewModel = _mapper.Map<PathwaySpecialismsViewModel>(pathwaySpecialisms);

            if (pathwaySpecialismsViewModel != null)
                pathwaySpecialismsViewModel.Specialisms = pathwaySpecialismsViewModel.Specialisms.OrderBy(x => x.DisplayName).ToList();

            return pathwaySpecialismsViewModel;
        }

        public async Task<UlnRegistrationNotFoundViewModel> FindUlnAsync(long aoUkprn, long Uln)
        {
            var response = await _internalApiClient.FindUlnAsync(aoUkprn, Uln);
            return _mapper.Map<UlnRegistrationNotFoundViewModel>(response);
        }

        public async Task<bool> AddRegistrationAsync(long aoUkprn, RegistrationViewModel model)
        {
            var registrationModel = _mapper.Map<RegistrationRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddRegistrationAsync(registrationModel);
        }

        public async Task<RegistrationDetailsViewModel> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var response = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, profileId, status);
            return _mapper.Map<RegistrationDetailsViewModel>(response);
        }

        public async Task<RegistrationAssessmentDetails> GetRegistrationAssessmentAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, status);
            return _mapper.Map<RegistrationAssessmentDetails>(response);
        }

        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            return await _internalApiClient.DeleteRegistrationAsync(aoUkprn, profileId);
        }

        public async Task<T> GetRegistrationProfileAsync<T>(long aoUkprn, int profileId, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active)
        {
            var response = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, profileId, status);
            return _mapper.Map<T>(response);
        }

        public async Task<ProviderChangeResponse> ProcessProviderChangesAsync(long aoUkprn, ChangeProviderViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);
            if (reg == null)
                return null;

            if (reg.ProviderUkprn == viewModel.SelectedProviderUkprn.ToLong())
            {
                return new ProviderChangeResponse { IsModified = false };
            }
            else
            {
                var providerPathways = await _internalApiClient.GetRegisteredProviderPathwayDetailsAsync(aoUkprn, viewModel.SelectedProviderUkprn.ToLong());
                if (providerPathways != null && providerPathways.Count > 0 && providerPathways.Any(p => p.Code.Equals(reg.PathwayLarId)))
                {
                    var request = _mapper.Map<ManageRegistration>(reg);
                    _mapper.Map(viewModel, request);
                    var isSuccess = await _internalApiClient.UpdateRegistrationAsync(request);
                    return new ProviderChangeResponse { ProfileId = request.ProfileId, Uln = request.Uln, IsModified = true, IsSuccess = isSuccess };
                }
                else
                {
                    return new ProviderChangeResponse { IsModified = true, IsCoreNotSupported = true };
                }
            }
        }

        public async Task<ManageRegistrationResponse> ProcessProfileNameChangeAsync(long aoUkprn, ChangeLearnersNameViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);
            if (reg == null)
                return null;

            if (viewModel.Firstname.Trim().Equals(reg.Firstname) && viewModel.Lastname.Trim().Equals(reg.Lastname))
                return new ManageRegistrationResponse { IsModified = false };

            var request = _mapper.Map<ManageRegistration>(reg);
            _mapper.Map(viewModel, request);

            var isSuccess = await _internalApiClient.UpdateRegistrationAsync(request);

            return new ManageRegistrationResponse { ProfileId = request.ProfileId, Uln = request.Uln, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<ManageRegistrationResponse> ProcessDateofBirthChangeAsync(long aoUkprn, ChangeDateofBirthViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);
            if (reg == null)
                return null;

            if (reg.DateofBirth == viewModel.DateofBirth.ToDateTime())
                return new ManageRegistrationResponse { IsModified = false };

            var request = _mapper.Map<ManageRegistration>(reg);
            _mapper.Map(viewModel, request);
            var isSuccess = await _internalApiClient.UpdateRegistrationAsync(request);

            return new ManageRegistrationResponse { ProfileId = request.ProfileId, Uln = request.Uln, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<ManageRegistrationResponse> ProcessSpecialismQuestionChangeAsync(long aoUkprn, ChangeSpecialismQuestionViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);

            if (reg == null || viewModel.HasLearnerDecidedSpecialism == null) return null;

            var request = _mapper.Map<ManageRegistration>(reg);
            _mapper.Map(viewModel, request);
            var isSuccess = await _internalApiClient.UpdateRegistrationAsync(request);
            return new ManageRegistrationResponse { ProfileId = request.ProfileId, Uln = request.Uln, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<ManageRegistrationResponse> ProcessSpecialismChangeAsync(long aoUkprn, ChangeSpecialismViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);
            if (reg == null)
                return null;

            var prevSpecialisms = reg.Specialisms.Select(x => x.Code.ToLowerInvariant());
            var currentSpecialisms = viewModel.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).SelectMany(s => s.Code.ToLowerInvariant().Split(Constants.PipeSeperator));
            var areSame = prevSpecialisms.Count() == currentSpecialisms.Count() && prevSpecialisms.All(x => currentSpecialisms.Contains(x));

            if (areSame)
                return new ManageRegistrationResponse { IsModified = false };

            var request = _mapper.Map<ManageRegistration>(reg);
            _mapper.Map(viewModel, request);
            var isSuccess = await _internalApiClient.UpdateRegistrationAsync(request);

            return new ManageRegistrationResponse { ProfileId = request.ProfileId, Uln = request.Uln, IsModified = true, IsSuccess = isSuccess };
        }

        public async Task<ChangeCoreQuestionViewModel> GetRegistrationChangeCoreQuestionDetailsAsync(long aoUkprn, int profileId)
        {
            var response = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, profileId);
            return _mapper.Map<ChangeCoreQuestionViewModel>(response);
        }

        public async Task<WithdrawRegistrationResponse> WithdrawRegistrationAsync(long aoUkprn, WithdrawRegistrationViewModel viewModel)
        {
            var model = _mapper.Map<WithdrawRegistrationRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            var isSuccess = await _internalApiClient.WithdrawRegistrationAsync(model);
            return new WithdrawRegistrationResponse { ProfileId = viewModel.ProfileId, Uln = viewModel.Uln, IsSuccess = isSuccess, IsRequestFromProviderAndCorePage = viewModel.IsRequestFromProviderAndCorePage };
        }

        public async Task<RejoinRegistrationResponse> RejoinRegistrationAsync(long aoUkprn, RejoinRegistrationViewModel viewModel)
        {
            var model = _mapper.Map<RejoinRegistrationRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            var isSuccess = await _internalApiClient.RejoinRegistrationAsync(model);
            return new RejoinRegistrationResponse { ProfileId = viewModel.ProfileId, Uln = viewModel.Uln, IsSuccess = isSuccess };
        }

        public async Task<ReregistrationResponse> ReregistrationAsync(long aoUkprn, ReregisterViewModel viewModel)
        {
            var reg = await _internalApiClient.GetRegistrationDetailsAsync(aoUkprn, viewModel.ReregisterProvider.ProfileId, RegistrationPathwayStatus.Withdrawn);
            if (reg == null)
                return null;

            var isCoreSameAsWithdrawnCore = reg.PathwayLarId.Equals(viewModel.ReregisterCore.SelectedCoreCode, StringComparison.InvariantCultureIgnoreCase);
            if (isCoreSameAsWithdrawnCore)
            {
                return new ReregistrationResponse { ProfileId = reg.ProfileId, Uln = reg.Uln, IsSelectedCoreSameAsWithdrawn = true };
            }

            var reregistrationRequest = _mapper.Map<ReregistrationRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            var isSuccess = await _internalApiClient.ReregistrationAsync(reregistrationRequest);
            return new ReregistrationResponse { ProfileId = reg.ProfileId, Uln = reg.Uln, IsSuccess = isSuccess };
        }

        public async Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync()
        {
            return await _internalApiClient.GetCurrentAcademicYearsAsync();
        }

        public async Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync()
        {
            return await _internalApiClient.GetAcademicYearsAsync();
        }

        public Task<IList<DataExportResponse>> GenerateRegistrationsExportAsync(long aoUkprn, string requestedBy)
        {
            return _internalApiClient.GenerateDataExportAsync(aoUkprn, DataExportType.Registrations, requestedBy);
        }

        public Task<IList<DataExportResponse>> GeneratePendingWithdrawalsExportAsync(long aoUkprn, string requestedBy)
        {
            return _internalApiClient.GenerateDataExportAsync(aoUkprn, DataExportType.PendingWithdrawals, requestedBy);
        }

        public Task<Stream> GetRegistrationsDataFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            string fileStreamNotFoundMessage = $"No FileStream found to download registrations data. Method: {nameof(GetRegistrationsDataFileAsync)}";
            return GetDataFileAsync(aoUkprn, blobUniqueReference, DataExportType.Registrations, fileStreamNotFoundMessage);
        }

        public Task<Stream> GetPendingWithdrawalsDataFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            string fileStreamNotFoundMessage = $"No FileStream found to download pending withdrawals data. Method: {nameof(GetPendingWithdrawalsDataFileAsync)}";
            return GetDataFileAsync(aoUkprn, blobUniqueReference, DataExportType.PendingWithdrawals, fileStreamNotFoundMessage);
        }

        private async Task<Stream> GetDataFileAsync(long aoUkprn, Guid blobUniqueReference, DataExportType dataExportType, string fileStreamNotFoundMessage)
        {
            string sourceFilePath = $"{aoUkprn}/{dataExportType}";

            Stream fileStream = await DownloadFileAsync(blobUniqueReference, sourceFilePath);
            if (fileStream == null)
            {
                string blobReadError = $"{fileStreamNotFoundMessage}(ContainerName: {DocumentType.Registrations}, BlobFileName = {blobUniqueReference}, SourceFilePath = {sourceFilePath})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }

            return fileStream;
        }

        private Task<Stream> DownloadFileAsync(Guid blobUniqueReference, string sourceFilePath)
        {
            return _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                SourceFilePath = sourceFilePath
            });
        }

        public async Task<ChangeAcademicYearResponse> ProcessChangeAcademicYearAsync(ChangeAcademicYearViewModel viewModel, int profileId)
        {
            var reviewChangeRequest = _mapper.Map<ChangeAcademicYearRequest>(viewModel);
            var response = await _internalApiClient.ProcessChangeAcademicYearAsync(reviewChangeRequest, profileId);
            return new ChangeAcademicYearResponse { ProfileId = profileId, Uln = viewModel.Uln, IsSuccess = response };
        }
    }
}
