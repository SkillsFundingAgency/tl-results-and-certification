using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class ResultsAndCertificationInternalApiClient : IResultsAndCertificationInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _internalApiUri;
        private readonly ITokenServiceClient _tokenServiceClient;
        private readonly bool _isDevevelopment;

        public ResultsAndCertificationInternalApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _isDevevelopment = configuration.IsDevevelopment;
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = configuration.ResultsAndCertificationInternalApiSettings.Uri.TrimEnd('/');
            _httpClient.BaseAddress = new Uri(_internalApiUri);
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetAllTLevelsUri, ukprn));
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            return await GetAsync<IEnumerable<AwardingOrganisationPathwayStatus>>(string.Format(ApiConstants.GetTlevelsByStatus, ukprn, statusId));
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var requestUri = string.Format(ApiConstants.TlevelDetailsUri, ukprn, id);
            return await GetAsync<TlevelPathwayDetails>(requestUri);
        }

        public async Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var requestUri = ApiConstants.VerifyTlevelUri;
            return await PutAsync<VerifyTlevelDetails, bool>(requestUri, model);
        }

        public async Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevel> model)
        {
            var requestUri = ApiConstants.AddProviderTlevelsUri;
            return await PostAsync<IList<ProviderTlevel>, bool>(requestUri, model);
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var requestUri = string.Format(ApiConstants.IsAnyProviderSetupCompletedUri, ukprn);
            return await GetAsync<bool>(requestUri);
        }

        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            var requestUri = string.Format(ApiConstants.FindProviderAsyncUri, name, isExactMatch);
            return await GetAsync<IEnumerable<ProviderMetadata>>(requestUri);
        }

        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetSelectProviderTlevelsUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetAllProviderTlevelsAsyncUri, aoUkprn, providerId);
            return await GetAsync<ProviderTlevels>(requestUri);
        }

        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetTqAoProviderDetailsAsyncUri, aoUkprn);
            return await GetAsync<IList<ProviderDetails>>(requestUri);
        }

        public async Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId)
        {
            var requestUri = string.Format(ApiConstants.GetTqProviderTlevelDetailsAsyncUri, aoUkprn, tqProviderId);
            return await GetAsync<ProviderTlevelDetails>(requestUri);
        }

        public async Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId)
        {
            var requestUri = string.Format(ApiConstants.RemoveTqProviderTlevelAsyncUri, aoUkprn, tqProviderId);
            return await DeleteAsync<bool>(requestUri);
        }

        public async Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId)
        {
            var requestUri = string.Format(ApiConstants.HasAnyTlevelSetupForProviderAsyncUri, aoUkprn, tlProviderId);
            return await GetAsync<bool>(requestUri);
        }

        public async Task<BulkProcessResponse> ProcessBulkRegistrationsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkRegistrationsUri;
            return await PostAsync<BulkProcessRequest, BulkProcessResponse>(requestUri, model);
        }

        public async Task<BulkProcessResponse> ProcessBulkWithdrawalsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkWithdrawalsUri;
            return await PostAsync<BulkProcessRequest, BulkProcessResponse>(requestUri, model);
        }

        public async Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long ukprn, Guid blobUniqueReference)
        {
            var requestUri = string.Format(ApiConstants.GetDocumentUploadHistoryDetailsAsyncUri, ukprn, blobUniqueReference);
            return await GetAsync<DocumentUploadHistoryDetails>(requestUri);
        }

        public async Task<IList<PathwayDetails>> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetRegisteredProviderPathwayDetailsAsyncUri, aoUkprn, providerUkprn);
            return await GetAsync<IList<PathwayDetails>>(requestUri);
        }

        public async Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            var requestUri = string.Format(ApiConstants.GetPathwaySpecialismsByPathwayLarIdAsyncUri, aoUkprn, pathwayLarId);
            return await GetAsync<PathwaySpecialisms>(requestUri);
        }

        public async Task<bool> AddRegistrationAsync(RegistrationRequest model)
        {
            var requestUri = ApiConstants.AddRegistrationUri;
            return await PostAsync<RegistrationRequest, bool>(requestUri, model);
        }

        public async Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln)
        {
            var requestUri = string.Format(ApiConstants.FindUlnUri, aoUkprn, uln);
            return await GetAsync<FindUlnResponse>(requestUri);
        }

        public async Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetRegistrationDetailsUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<RegistrationDetails>(requestUri);
        }

        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            var requestUri = string.Format(ApiConstants.DeleteRegistrationUri, aoUkprn, profileId);
            return await DeleteAsync<bool>(requestUri);
        }

        public async Task<bool> UpdateRegistrationAsync(ManageRegistration model)
        {
            var requestUri = ApiConstants.UpdateRegistrationUri;
            return await PutAsync<ManageRegistration, bool>(requestUri, model);
        }

        public async Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model)
        {
            var requestUri = ApiConstants.WithdrawRegistrationUri;
            return await PutAsync<WithdrawRegistrationRequest, bool>(requestUri, model);
        }

        public async Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model)
        {
            var requestUri = ApiConstants.RejoinRegistrationUri;
            return await PutAsync<RejoinRegistrationRequest, bool>(requestUri, model);
        }

        public async Task<bool> ReregistrationAsync(ReregistrationRequest model)
        {
            var requestUri = ApiConstants.ReregistrationUri;
            return await PostAsync<ReregistrationRequest, bool>(requestUri, model);
        }

        // Assessment Related endpoints
        public async Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkAssessmentsUri;
            return await PostAsync<BulkProcessRequest, BulkAssessmentResponse>(requestUri, model);
        }

        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType, string componentIds)
        {
            var requestUri = string.Format(ApiConstants.GetAvailableAssessmentSeriesUri, aoUkprn, profileId, (int)componentType, componentIds);
            return await GetAsync<AvailableAssessmentSeries>(requestUri);
        }

        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            var requestUri = ApiConstants.AddAssessmentEntryUri;
            return await PostAsync<AddAssessmentEntryRequest, AddAssessmentEntryResponse>(requestUri, request);
        }

        public async Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType)
        {
            var requestUri = string.Format(ApiConstants.GetActiveAssessmentEntryDetailsUri, aoUkprn, assessmentId, (int)componentType);
            return await GetAsync<AssessmentEntryDetails>(requestUri);
        }

        public async Task<IEnumerable<AssessmentEntryDetails>> GetActiveSpecialismAssessmentEntriesAsync(long aoUkprn, string specialismAssessmentIds)
        {
            var requestUri = string.Format(ApiConstants.GetActiveSpecialismAssessmentEntriesUri, aoUkprn, specialismAssessmentIds);
            return await GetAsync<IEnumerable<AssessmentEntryDetails>>(requestUri);
        }

        public async Task<bool> RemoveAssessmentEntryAsync(RemoveAssessmentEntryRequest model)
        {
            var requestUri = ApiConstants.RemoveAssessmentEntryUri;
            return await PutAsync<RemoveAssessmentEntryRequest, bool>(requestUri, model);
        }

        public async Task<IList<AssessmentSeriesDetails>> GetAssessmentSeriesAsync()
        {
            var requestUri = ApiConstants.GetAssessmentSeriesDetailsUri;
            return await GetAsync<IList<AssessmentSeriesDetails>>(requestUri);
        }

        // Assessment Series
        public async Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync()
        {
            var requestUri = ApiConstants.GetResultCalculationAssessmentUri;
            return await GetAsync<AssessmentSeriesDetails>(requestUri);
        }

        // Results endpoints
        public async Task<BulkResultResponse> ProcessBulkResultsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkResultsUri;
            return await PostAsync<BulkProcessRequest, BulkResultResponse>(requestUri, model);
        }

        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetResultDetailsUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<ResultDetails>(requestUri);
        }

        public async Task<AddResultResponse> AddResultAsync(AddResultRequest request)
        {
            var requestUri = ApiConstants.AddResultUri;
            return await PostAsync<AddResultRequest, AddResultResponse>(requestUri, request);
        }

        public async Task<ChangeResultResponse> ChangeResultAsync(ChangeResultRequest model)
        {
            var requestUri = ApiConstants.ChangeResultUri;
            return await PutAsync<ChangeResultRequest, ChangeResultResponse>(requestUri, model);
        }

        # region Common controller
        public async Task<IList<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory)
        {
            var requestUri = string.Format(ApiConstants.GetLookupDataUri, (int)lookupCategory);
            return await GetAsync<IList<LookupData>>(requestUri);
        }

        // LoggedIn User endpoint
        public async Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn)
        {
            var requestUri = string.Format(ApiConstants.GetLoggedInUserTypeInfoUri, ukprn);
            return await GetAsync<LoggedInUserTypeInfo>(requestUri);
        }

        public async Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync()
        {
            return await GetAsync<IEnumerable<AcademicYear>>(ApiConstants.GetCurrentAcademicYears);
        }

        public async Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync()
        {
            return await GetAsync<IEnumerable<AcademicYear>>(ApiConstants.GetAcademicYears);
        }

        #endregion

        // Training Provider endpoints
        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            return await PostAsync<SearchLearnerRequest, PagedResponse<SearchLearnerDetail>>(ApiConstants.SearchLearnerDetailsUri, request);
        }

        public async Task<SearchLearnerFilters> GetSearchLearnerFiltersAsync(long providerUkprn)
        {
            var requestUri = string.Format(ApiConstants.SearchLearnerFiltersUri, providerUkprn);
            return await GetAsync<SearchLearnerFilters>(requestUri);
        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            var requestUri = string.Format(ApiConstants.FindLearnerRecordUri, providerUkprn, uln);
            return await GetAsync<FindLearnerRecord>(requestUri);
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var requestUri = string.Format(ApiConstants.GetLearnerRecordDetailsUri, providerUkprn, profileId, pathwayId);
            return await GetAsync<LearnerRecordDetails>(requestUri);
        }

        public async Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request)
        {
            return await PutAsync<UpdateLearnerSubjectRequest, bool>(ApiConstants.UpdateLearnerSubjectUri, request);
        }

        // Provider Address endpoints
        public async Task<bool> AddAddressAsync(AddAddressRequest request)
        {
            return await PostAsync<AddAddressRequest, bool>(ApiConstants.AddAddressUri, request);
        }

        public async Task<Address> GetAddressAsync(long providerUkprn)
        {
            var requestUri = string.Format(ApiConstants.GetAddressUri, providerUkprn);
            return await GetAsync<Address>(requestUri);
        }

        // Provider Statement Of Achievement endpoints
        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            var requestUri = string.Format(ApiConstants.FindSoaLearnerRecordUri, providerUkprn, uln);
            return await GetAsync<FindSoaLearnerRecord>(requestUri);
        }

        public async Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            var requestUri = string.Format(ApiConstants.GetSoaLearnerRecordDetailsUri, providerUkprn, profileId);
            return await GetAsync<SoaLearnerRecordDetails>(requestUri);
        }

        public async Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(SoaPrintingRequest request)
        {
            return await PostAsync<SoaPrintingRequest, SoaPrintingResponse>(ApiConstants.CreateSoaPrintingRequestUri, request);
        }

        public async Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId)
        {
            var requestUri = string.Format(ApiConstants.GetPrintRequestSnapshotUri, providerUkprn, profileId, pathwayId);
            return await GetAsync<PrintRequestSnapshot>(requestUri);
        }

        // Provider Replacement Document endpoints
        public async Task<bool> CreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request)
        {
            return await PostAsync<ReplacementPrintRequest, bool>(ApiConstants.CreateReplacementDocumentPrintingRequestUri, request);
        }

        #region PRS
        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null)
        {
            var requestUri = uln != null ?
                string.Format(ApiConstants.FindPrsLearnerRecordUri, aoUkprn, uln) :
                string.Format(ApiConstants.FindPrsLearnerRecordByProfileIdUri, aoUkprn, profileId);

            return await GetAsync<FindPrsLearnerRecord>(requestUri);
        }

        public async Task<bool> PrsActivityAsync(PrsActivityRequest request)
        {
            var requestUri = ApiConstants.PrsActivityUri;
            return await PostAsync<PrsActivityRequest, bool>(requestUri, request);
        }

        public async Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request)
        {
            var requestUri = ApiConstants.PrsGradeChangeRequestUri;
            return await PostAsync<PrsGradeChangeRequest, bool>(requestUri, request);
        }

        public async Task<BulkProcessResponse> ProcessBulkRommsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkRommsUri;
            return await PostAsync<BulkProcessRequest, BulkProcessResponse>(requestUri, model);
        }

        #endregion

        #region IndustryPlacement

        public async Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null)
        {
            var requestUri = string.Format(ApiConstants.GetIpLookupDataUri, (int)ipLookupType, pathwayId);
            return await GetAsync<IList<IpLookupData>>(requestUri);
        }

        public async Task<bool> ProcessIndustryPlacementDetailsAsync(IndustryPlacementRequest request)
        {
            return await PostAsync<IndustryPlacementRequest, bool>(ApiConstants.ProcessIndustryPlacementDetailsUri, request);
        }

        #endregion

        public async Task<LearnerRecord> GetLearnerRecordAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var requestUri = string.Format(ApiConstants.GetLearnerRecordUri, aoUkprn, profileId, (int?)status);
            return await GetAsync<LearnerRecord>(requestUri);
        }

        public async Task<IList<DataExportResponse>> GenerateDataExportAsync(long aoUkprn, DataExportType dataExportType, string requestedBy)
        {
            var requestUri = string.Format(ApiConstants.GetDataExportUri, aoUkprn, (int)dataExportType, requestedBy);
            return await GetAsync<IList<DataExportResponse>>(requestUri);
        }

        public async Task<DataExportResponse> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy)
        {
            var requestUri = string.Format(ApiConstants.DownloadOverallResultsDataUri, providerUkprn, requestedBy);
            return await GetAsync<DataExportResponse>(requestUri);
        }

        public async Task<DataExportResponse> DownloadOverallResultSlipsDataAsync(long providerUkprn, string requestedBy)
        {
            var requestUri = string.Format(ApiConstants.DownloadOverallResultSlipsDataUri, providerUkprn, requestedBy);
            return await GetAsync<DataExportResponse>(requestUri);
        }

        public async Task<DataExportResponse> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, int profileId, string requestedBy)
        {
            var requestUri = string.Format(ApiConstants.DownloadLearnerOverallResultSlipsDataUri, providerUkprn, profileId, requestedBy);
            return await GetAsync<DataExportResponse>(requestUri);
        }

        #region Industry Placement Bulk Upload

        public async Task<BulkIndustryPlacementResponse> ProcessBulkIndustryPlacementsAsync(BulkProcessRequest model)
        {
            var requestUri = ApiConstants.ProcessBulkIndustryPlacementsUri;
            return await PostAsync<BulkProcessRequest, BulkIndustryPlacementResponse>(requestUri, model);
        }

        #endregion

        #region Registration pending withdrawal

        public async Task<bool> SetRegistrationAsPendingWithdrawalAsync(SetRegistrationAsPendingWithdrawalRequest model)
        {
            var requestUri = ApiConstants.SetRegistrationAsPendingWithdrawalUri;
            return await PutAsync<SetRegistrationAsPendingWithdrawalRequest, bool>(requestUri, model);
        }

        public async Task<bool> ReinstateRegistrationFromPendingWithdrawalAsync(ReinstateRegistrationFromPendingWithdrawalRequest model)
        {
            var requestUri = ApiConstants.ReinstateRegistrationFromPendingWithdrawalUri;
            return await PutAsync<ReinstateRegistrationFromPendingWithdrawalRequest, bool>(requestUri, model);
        }

        #endregion

        #region Admin dashboard

        public Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync()
        {
            var requestUri = ApiConstants.GetAdminSearchLearnerFiltersUri;
            return GetAsync<AdminSearchLearnerFilters>(requestUri);
        }

        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            var requestUri = ApiConstants.GetAdminSearchLearnerDetailsUri;
            return PostAsync<AdminSearchLearnerRequest, PagedResponse<AdminSearchLearnerDetail>>(requestUri, request);
        }

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int pathwayId)
        {
            var requestUri = string.Format(ApiConstants.GetAdminLearnerRecordUri, pathwayId);
            return await GetAsync<AdminLearnerRecord>(requestUri);
        }

        public Task<IList<int>> GetAllowedChangeAcademicYearsAsync(int learnerAcademicYear, int pathwayStartYear)
        {
            var requestUri = string.Format(ApiConstants.GetAllowedChangeAcademicYearsUri, learnerAcademicYear, pathwayStartYear);
            return GetAsync<IList<int>>(requestUri);
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request)
        {
            return await PostAsync<ReviewChangeStartYearRequest, bool>(ApiConstants.ProcessChangeStartYearUri, request);
        }

        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            return await PostAsync<ReviewChangeIndustryPlacementRequest, bool>(ApiConstants.ProcessChangeIPUri, request);
        }

        public async Task<bool> ProcessAddCoreAssessmentRequestAsync(ReviewAddCoreAssessmentRequest request)
        {
            return await PostAsync<ReviewAddCoreAssessmentRequest, bool>(ApiConstants.ProcessAddCoreAssessmentUri, request);
        }

        public async Task<bool> ProcessAddSpecialismAssessmentRequestAsync(ReviewAddSpecialismAssessmentRequest request)
        {
            return await PostAsync<ReviewAddSpecialismAssessmentRequest, bool>(ApiConstants.ProcessAddSpecialismAssessmentUri, request);
        }

        public async Task<bool> RemoveAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryRequest request)
        {
            return await PostAsync<ReviewRemoveCoreAssessmentEntryRequest, bool>(ApiConstants.ReviewRemoveCoreAssessmentEntryUri, request);
        }

        public async Task<bool> RemoveSpecialAssessmentEntryAsync(ReviewRemoveSpecialismAssessmentEntryRequest request)
        {
            return await PostAsync<ReviewRemoveSpecialismAssessmentEntryRequest, bool>(ApiConstants.ReviewRemoveSpecialismAssessmentEntryUri, request);
        }

        public Task<bool> ProcessAdminAddPathwayResultAsync(AddPathwayResultRequest request)
            => PostAsync<AddPathwayResultRequest, bool>(ApiConstants.ProcessAdminAddPathwayResultUri, request);

        public Task<bool> ProcessAdminAddSpecialismResultAsync(AddSpecialismResultRequest request)
            => PostAsync<AddSpecialismResultRequest, bool>(ApiConstants.ProcessAdminAddSpecialismResultUri, request);

        #endregion

        #region Admin change log

        public Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request)
        {
            var requestUri = ApiConstants.SearchChangeLogsUri;
            return PostAsync<AdminSearchChangeLogRequest, PagedResponse<AdminSearchChangeLog>>(requestUri, request);
        }

        public Task<bool> ProcessAdminChangePathwayResultAsync(ChangePathwayResultRequest request)
          => PostAsync<ChangePathwayResultRequest, bool>(ApiConstants.ProcessAdminChangePathwayResult, request);

        public Task<bool> ProcessAdminChangeSpecialismResultAsync(ChangeSpecialismResultRequest request)
          => PostAsync<ChangeSpecialismResultRequest, bool>(ApiConstants.ProcessAdminChangeSpecialismResult, request);

        public Task<AdminChangeLogRecord> GetAdminChangeLogRecordAsync(int changeLogId)
            => GetAsync<AdminChangeLogRecord>(string.Format(ApiConstants.GetAdminChangeLogRecord, changeLogId));

        #endregion

        #region Admin post results

        public Task<bool> ProcessAdminOpenPathwayRommAsync(OpenPathwayRommRequest request)
             => PostAsync<OpenPathwayRommRequest, bool>(ApiConstants.ProcessAdminOpenPathwayRomm, request);

        public Task<bool> ProcessAdminOpenSpecialismRommAsync(OpenSpecialismRommRequest request)
             => PostAsync<OpenSpecialismRommRequest, bool>(ApiConstants.ProcessAdminOpenSpecialismRomm, request);

        public Task<bool> ProcessAdminReviewChangesRommOutcomeCoreAsync(ReviewChangesRommOutcomeCoreRequest request)
            => PostAsync<ReviewChangesRommOutcomeCoreRequest, bool>(ApiConstants.ProcessAdminReviewChangesRommOutcomeCore, request);

        public Task<bool> ProcessAdminReviewChangesRommOutcomeSpecialismAsync(ReviewChangesRommOutcomeSpecialismRequest request)
          => PostAsync<ReviewChangesRommOutcomeSpecialismRequest, bool>(ApiConstants.ProcessAdminReviewChangesRommOutcomeSpecialism, request);

        public Task<bool> ProcessAdminOpenCoreAppealAsync(OpenCoreAppealRequest request)
             => PostAsync<OpenCoreAppealRequest, bool>(ApiConstants.ProcessAdminOpenCoreAppeal, request);

        public Task<bool> ProcessAdminOpenSpecialismAppealAsync(OpenSpecialismAppealRequest request)
             => PostAsync<OpenSpecialismAppealRequest, bool>(ApiConstants.ProcessAdminOpenSpecialismAppeal, request);

        public Task<bool> ProcessAdminReviewChangesAppealOutcomeCoreAsync(ReviewChangesAppealOutcomeCoreRequest request)
            => PostAsync<ReviewChangesAppealOutcomeCoreRequest, bool>(ApiConstants.ProcessAdminReviewChangesAppealOutcomeCore, request);

        public Task<bool> ProcessAdminReviewChangesAppealOutcomeSpecialismAsync(ReviewChangesAppealOutcomeSpecialismRequest request)
          => PostAsync<ReviewChangesAppealOutcomeSpecialismRequest, bool>(ApiConstants.ProcessAdminReviewChangesAppealOutcomeSpecialism, request);

        #endregion

        #region Registration search

        public Task<SearchRegistrationFilters> GetSearchRegistrationFiltersAsync()
            => GetAsync<SearchRegistrationFilters>(ApiConstants.GetSearchRegistrationFiltersUri);

        public Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request)
            => PostAsync<SearchRegistrationRequest, PagedResponse<SearchRegistrationDetail>>(ApiConstants.SearchRegistrationDetailsUri, request);

        #endregion

        #region Provider registrations

        public Task<IList<int>> GetProviderRegistrationsAvailableStartYearsAsync()
            => GetAsync<IList<int>>(ApiConstants.GetAvailableStartYearsUri);

        public Task<DataExportResponse> GetProviderRegistrationsAsync(GetProviderRegistrationsRequest request)
            => PostAsync<GetProviderRegistrationsRequest, DataExportResponse>(ApiConstants.GetRegistrationsUri, request);

        #endregion

        #region Request replacement document

        public Task<bool> ProcessAdminCreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request)
           => PostAsync<ReplacementPrintRequest, bool>(ApiConstants.ProcessAdminCreateReplacementDocumentPrintingRequestUri, request);

        #endregion

        #region Admin providers

        public Task<GetProviderResponse> GetProviderAsync(int providerId)
        {
            var requestUri = string.Format(ApiConstants.GetProvider, providerId);
            return GetAsync<GetProviderResponse>(requestUri);
        }

        public Task<AddProviderResponse> AddProviderAsync(AddProviderRequest request)
            => PostAsync<AddProviderRequest, AddProviderResponse>(ApiConstants.AddProvider, request);

        public Task<UpdateProviderResponse> UpdateProviderAsync(UpdateProviderRequest request)
            => PutAsync<UpdateProviderRequest, UpdateProviderResponse>(ApiConstants.UpdateProvider, request);

        #endregion

        #region Dashboard

        public Task<IEnumerable<string>> GetAwardingOrganisationBanners()
            => GetAsync<IEnumerable<string>>(ApiConstants.GetAwardingOrganisationBanners);

        public Task<IEnumerable<string>> GetProviderBanners()
            => GetAsync<IEnumerable<string>>(ApiConstants.GetProviderBanners);

        #endregion

        #region Admin banners

        public Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request)
            => PostAsync<AdminSearchBannerRequest, PagedResponse<SearchBannerDetail>>(ApiConstants.SearchBanners, request);

        public Task<GetBannerResponse> GetBannerAsync(int bannerId)
        {
            var requestUri = string.Format(ApiConstants.GetBanner, bannerId);
            return GetAsync<GetBannerResponse>(requestUri);
        }

        public Task<AddBannerResponse> AddBannerAsync(AddBannerRequest request)
            => PostAsync<AddBannerRequest, AddBannerResponse>(ApiConstants.AddBanner, request);

        public Task<bool> UpdateBannerAsync(UpdateBannerRequest request)
            => PutAsync<UpdateBannerRequest, bool>(ApiConstants.UpdateBanner, request);

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the bearer token.
        /// </summary>
        private async Task SetBearerToken()
        {
            if (!_isDevevelopment)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _tokenServiceClient.GetInternalApiToken());
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Puts the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private async Task<TResponse> PutAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            await SetBearerToken();
            var response = await _httpClient.PutAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<TResponse> DeleteAsync<TResponse>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, IsoDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Gets the microsoft date format settings.
        /// </summary>
        /// <value>
        /// The microsoft date format settings.
        /// </value>
        private static JsonSerializerSettings IsoDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };
            }
        }

        #endregion
    }
}