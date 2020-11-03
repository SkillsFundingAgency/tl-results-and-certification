using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrations
{
    public abstract class TestSetup : BaseTest<BulkRegistrationLoader>
    {
        protected ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> CsvService;
        protected IRegistrationService RegistrationService;
        protected IBlobStorageService BlobService;
        protected IDocumentUploadHistoryService DocumentUploadHistoryService;
        protected ILogger<BulkRegistrationLoader> Logger;
        private BulkRegistrationLoader _loader;
        protected BulkRegistrationRequest Request;
        protected BulkRegistrationResponse Response { get; private set; }
        protected long AoUkprn = 1234567891;
        protected Guid BlobUniqueRef;

        public override void Setup()
        {
            CsvService = Substitute.For<ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>();
            RegistrationService = Substitute.For<IRegistrationService>();
            BlobService = Substitute.For<IBlobStorageService>();
            Logger = Substitute.For<ILogger<BulkRegistrationLoader>>();
            DocumentUploadHistoryService = Substitute.For<IDocumentUploadHistoryService>();

            BlobUniqueRef = Guid.NewGuid();

            Request = new BulkRegistrationRequest
            {
                AoUkprn = 1234567891,
                BlobFileName = "registrations.csv",
                BlobUniqueReference = BlobUniqueRef,
                DocumentType = DocumentType.Registrations,
                FileType = FileType.Csv,
                PerformedBy = "TestUser",
            };
        }

        public async override Task When()
        {
            _loader = new BulkRegistrationLoader(CsvService, RegistrationService, BlobService, DocumentUploadHistoryService, Logger);
            Response = await _loader.ProcessAsync(Request);
        }

        public List<BulkProcessValidationError> ExtractExpectedErrors(CsvResponseModel<RegistrationCsvRecordResponse> csvResponse)
        {
            if (csvResponse.IsDirty)
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = csvResponse.ErrorMessage } };

            var errors = new List<BulkProcessValidationError>();
            var invalidReg = csvResponse.Rows?.Where(x => !x.IsValid).ToList();
            invalidReg.ForEach(x => { errors.AddRange(x.ValidationErrors); });

            return errors;
        }
    }
}
