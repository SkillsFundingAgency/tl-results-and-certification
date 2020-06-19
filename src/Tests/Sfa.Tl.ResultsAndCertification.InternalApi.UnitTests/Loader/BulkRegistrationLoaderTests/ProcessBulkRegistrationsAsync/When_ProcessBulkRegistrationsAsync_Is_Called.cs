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

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public abstract class When_ProcessBulkRegistrationsAsync_Is_Called : BaseTest<BulkRegistrationLoader>
    {
        protected ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> CsvService;
        private IRegistrationService RegistrationService;
        protected IBlobStorageService BlobService;
        protected IDocumentUploadHistoryService DocumentUploadHistoryService;
        protected ILogger<BulkRegistrationLoader> Logger;

        private BulkRegistrationLoader _loader;

        protected BulkRegistrationRequest Request;
        protected Task<BulkRegistrationResponse> Response { get; private set; }
        

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
                AoUkprn = 1234,
                BlobFileName = "registrations.csv",
                BlobUniqueReference = BlobUniqueRef,
                DocumentType = DocumentType.Registrations,
                FileType = FileType.Csv,
                PerformedBy = "TestUser",
            };
        }

        public override void When()
        {
            _loader = new BulkRegistrationLoader(CsvService, RegistrationService, BlobService, DocumentUploadHistoryService, Logger);
            Response = _loader.ProcessBulkRegistrationsAsync(Request);
        }

        public List<RegistrationValidationError> ExtractExpectedErrors(CsvResponseModel<RegistrationCsvRecordResponse> csvResponse)
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
