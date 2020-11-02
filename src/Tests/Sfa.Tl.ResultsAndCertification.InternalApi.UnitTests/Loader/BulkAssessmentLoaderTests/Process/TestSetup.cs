using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public abstract class TestSetup : BaseTest<BulkAssessmentLoader>
    {
        protected ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> CsvService;
        protected IAssessmentService AssessmentService;
        protected IBlobStorageService BlobService;
        protected IDocumentUploadHistoryService DocumentUploadHistoryService;
        protected ILogger<BulkAssessmentLoader> Logger;
        private BulkAssessmentLoader _loader;
        protected BulkRegistrationRequest Request;
        protected BulkRegistrationResponse Response { get; private set; }
        protected long AoUkprn = 1234567891;
        protected Guid BlobUniqueRef;

        public override void Setup()
        {
            CsvService = Substitute.For<ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>();
            AssessmentService = Substitute.For<IAssessmentService>();
            BlobService = Substitute.For<IBlobStorageService>();
            Logger = Substitute.For<ILogger<BulkAssessmentLoader>>();
            DocumentUploadHistoryService = Substitute.For<IDocumentUploadHistoryService>();

            BlobUniqueRef = Guid.NewGuid();

            Request = new BulkRegistrationRequest
            {
                AoUkprn = 1234567891,
                BlobFileName = "assessments.csv",
                BlobUniqueReference = BlobUniqueRef,
                DocumentType = DocumentType.Assessments,
                FileType = FileType.Csv,
                PerformedBy = "TestUser",
            };
        }

        public async override Task When()
        {
            _loader = new BulkAssessmentLoader(CsvService, BlobService, DocumentUploadHistoryService, Logger);
            Response = await _loader.ProcessAsync(Request);
        }

        public List<RegistrationValidationError> ExtractExpectedErrors(CsvResponseModel<AssessmentCsvRecordResponse> csvResponse)
        {
            if (csvResponse.IsDirty)
                return new List<RegistrationValidationError> { new RegistrationValidationError { ErrorMessage = csvResponse.ErrorMessage } };

            var errors = new List<RegistrationValidationError>();
            var invalidReg = csvResponse.Rows?.Where(x => !x.IsValid).ToList();
            invalidReg.ForEach(x => { errors.AddRange(x.ValidationErrors); });

            return errors;
        }
    }
}
