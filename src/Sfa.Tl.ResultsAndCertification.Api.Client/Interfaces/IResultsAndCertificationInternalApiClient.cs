using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        // Tlevels
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn);
        Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
        Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model);
        Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId);

        // Providers
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch);
        Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevel> model);
        Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn);
        Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId);
        Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId);
        Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId);

        //Registrations
        Task<BulkProcessResponse> ProcessBulkRegistrationsAsync(BulkProcessRequest model);
        Task<IList<PathwayDetails>> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
        Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);
        
        // Manage registrations
        Task<bool> UpdateRegistrationAsync(ManageRegistration model);
        Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model);
        Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model);
        Task<bool> ReregistrationAsync(ReregistrationRequest model);

        // Assessments
        Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest model);
        Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType, string componentIds);
        Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType);
        Task<IEnumerable<AssessmentEntryDetails>> GetActiveSpecialismAssessmentEntriesAsync(long aoUkprn, string specialismAssessmentIds);
        Task<bool> RemoveAssessmentEntryAsync(RemoveAssessmentEntryRequest model);
        Task<IList<AssessmentSeriesDetails>> GetAssessmentSeriesAsync();

        // Results
        Task<BulkResultResponse> ProcessBulkResultsAsync(BulkProcessRequest model);
        Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<AddResultResponse> AddResultAsync(AddResultRequest request);
        Task<ChangeResultResponse> ChangeResultAsync(ChangeResultRequest model);

        // DocumentUploadHistory
        Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long aoUkprn, Guid blobUniqueReference);
        Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request);

        // TraningProvider
        Task<FindLearnerRecord> FindLearnerRecordAsync(long aoUkprn, long uln, bool? evaluateSendConfirmation = false);
        Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request);
        Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest model);
        Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request);

        // ProviderAddress
        Task<bool> AddAddressAsync(AddAddressRequest request);
        Task<Address> GetAddressAsync(long providerUkprn);

        // Provider Statement Of Achievement
        Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln);
        Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId);
        Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(SoaPrintingRequest request);
        Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId);

        // Post Results Service 
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null);
        Task<bool> PrsActivityAsync(PrsActivityRequest request);
        Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request);

        #region IndustryPlacement
        Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null);
        Task<IpTempFlexNavigation> GetTempFlexNavigationAsync(int pathwayId, int academicYear);

        #endregion

        // Common
        Task<IList<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory);
        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);
        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
        Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync();

        Task<LearnerRecord> GetLearnerRecordAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        
        Task<IList<DataExportResponse>> GenerateDataExportAsync(long aoUkprn, DataExportType dataExportType, string requestedBy);
    }
}