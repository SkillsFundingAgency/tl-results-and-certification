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
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkIndustryPlacementLoaderTests.Process
{
    public abstract class TestSetup : BaseTest<BulkIndustryPlacementLoader>
    {
        protected ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse> CsvService;
        protected IIndustryPlacementService IndustryPlacementService;
        protected IBlobStorageService BlobService;
        protected IDocumentUploadHistoryService DocumentUploadHistoryService;
        protected ILogger<BulkIndustryPlacementLoader> Logger;
        private BulkIndustryPlacementLoader _loader;
        protected BulkProcessRequest Request;
        protected BulkProcessResponse Response { get; private set; }
        protected long AoUkprn = 1234567891;
        protected Guid BlobUniqueRef;

        public override void Setup()
        {
            CsvService = Substitute.For<ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>>();
            IndustryPlacementService = Substitute.For<IIndustryPlacementService>();
            BlobService = Substitute.For<IBlobStorageService>();
            Logger = Substitute.For<ILogger<BulkIndustryPlacementLoader>>();
            DocumentUploadHistoryService = Substitute.For<IDocumentUploadHistoryService>();

            BlobUniqueRef = Guid.NewGuid();

            Request = new BulkProcessRequest
            {
                AoUkprn = 1234567891,
                BlobFileName = "IndustryPlacements.csv",
                BlobUniqueReference = BlobUniqueRef,
                DocumentType = DocumentType.IndustryPlacements,
                FileType = FileType.Csv,
                PerformedBy = "TestUser",
            };
        }

        public async override Task When()
        {
            _loader = new BulkIndustryPlacementLoader(CsvService, IndustryPlacementService, BlobService, DocumentUploadHistoryService, Logger);
            Response = await _loader.ProcessAsync(Request);
        }

        public List<BulkProcessValidationError> ExtractExpectedErrors(CsvResponseModel<IndustryPlacementCsvRecordResponse> csvResponse)
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
