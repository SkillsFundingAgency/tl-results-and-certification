using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DownloadOverallResultsLoaderTests.DownloadOverallResultsData
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Guid _fileGuid;
        private string _expectedFileContent;

        public override void Given()
        {
            _expectedFileContent = "Test File for download overall results";
            ExpectedApiResult = new MemoryStream(Encoding.ASCII.GetBytes(_expectedFileContent));
            
            _fileGuid = Guid.NewGuid();
            var apiResponse = new DataExportResponse { FileSize = 100, IsDataFound = true, ComponentType = ComponentType.NotSpecified, BlobUniqueReference = _fileGuid };
            InternalApiClient.DownloadOverallResultsDataAsync(providerUkprn, RequestedBy)
                .Returns(apiResponse);

            BlobStorageService.DownloadFileAsync(Arg.Is<BlobStorageData>(x =>
                                                             x.ContainerName == DocumentType.OverallResults.ToString() &&
                                                             x.BlobFileName == $"{_fileGuid}.{FileType.Csv}" &&
                                                             x.SourceFilePath == $"{providerUkprn}")).Returns(ExpectedApiResult);

        }


        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).DownloadOverallResultsDataAsync(providerUkprn, RequestedBy);

            BlobStorageService.Received(1).DownloadFileAsync(Arg.Is<BlobStorageData>(x =>
                                                             x.ContainerName == DocumentType.OverallResults.ToString() &&
                                                             x.BlobFileName == $"{_fileGuid}.{FileType.Csv}" &&
                                                             x.SourceFilePath == $"{providerUkprn}"));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            string actualFileContent;
            using (StreamReader reader = new StreamReader(ActualResult))
            {
                actualFileContent = reader.ReadToEnd();
            }

            actualFileContent.Should().Be(actualFileContent);
        }
    }
}
