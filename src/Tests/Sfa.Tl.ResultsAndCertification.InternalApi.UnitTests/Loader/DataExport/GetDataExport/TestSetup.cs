﻿using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.GetDataExport
{
    public abstract class TestSetup : BaseTest<DataExportLoader>
    {
        protected IDataExportRepository DataExportRepository;
        protected IBlobStorageService BlobService;
        protected IDownloadOverallResultsService DownloadOverallResultsService;
        protected IResultSlipsGeneratorService ResultSlipsGeneratorService;
        private DataExportLoader _loader;
        protected IList<DataExportResponse> Response { get; private set; }

        // params
        protected long AoUkprn = 1234567891;
        protected DataExportType DataExportType;
        protected string RequestedBy = "Test User";

        public override void Setup()
        {
            DataExportRepository = Substitute.For<IDataExportRepository>();
            BlobService = Substitute.For<IBlobStorageService>();
            DownloadOverallResultsService = Substitute.For<IDownloadOverallResultsService>();
        }

        public async override Task When()
        {
            _loader = new DataExportLoader(DataExportRepository, BlobService, DownloadOverallResultsService, ResultSlipsGeneratorService);
            Response = await _loader.ProcessDataExportAsync(AoUkprn, DataExportType, RequestedBy);
        }
    }
}