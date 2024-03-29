﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIndustryPlacementValidationErrorsFile
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference);
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
        }
    }
}
