﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DownloadOverallResultsLoaderTests.DownloadOverallResultSlipsData
{
    public class When_Blob_DownloadFile_IsNull : TestSetup
    {
        private Guid _fileGuid;

        public override void Given()
        {
            _fileGuid = Guid.NewGuid();
            var apiResponse = new DataExportResponse { FileSize = 100, IsDataFound = true, ComponentType = ComponentType.NotSpecified, BlobUniqueReference = _fileGuid };
            InternalApiClient.DownloadOverallResultSlipsDataAsync(providerUkprn, RequestedBy)
                .Returns(apiResponse);

            BlobStorageService.DownloadFileAsync(Arg.Is<BlobStorageData>(x =>
                                                             x.ContainerName == DocumentType.ResultSlips.ToString() &&
                                                             x.BlobFileName == $"{_fileGuid}.{FileType.Pdf}" &&
                                                             x.SourceFilePath == $"{providerUkprn}")).Returns(ExpectedApiResult);

        }


        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).DownloadOverallResultSlipsDataAsync(providerUkprn, RequestedBy);

            BlobStorageService.Received(1).DownloadFileAsync(Arg.Is<BlobStorageData>(x =>
                                                             x.ContainerName == DocumentType.ResultSlips.ToString() &&
                                                             x.BlobFileName == $"{_fileGuid}.{FileType.Pdf}" &&
                                                             x.SourceFilePath == $"{providerUkprn}"));
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
