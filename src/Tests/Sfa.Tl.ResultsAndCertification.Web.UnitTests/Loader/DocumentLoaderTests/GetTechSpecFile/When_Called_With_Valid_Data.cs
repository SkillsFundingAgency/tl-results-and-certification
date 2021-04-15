using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetTechSpecFile
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BlobStorageService.DownloadFileAsync(Arg.Is<BlobStorageData>(x => 
                                                            x.ContainerName == DocumentType.Documents.ToString() && 
                                                            x.BlobFileName == FileName && 
                                                            x.SourceFilePath == $"{BlobStorageConstants.TechSpecFolderName}/{FolderName}"))
                                                .Returns(new MemoryStream());
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Is<BlobStorageData>(x =>
                                                            x.ContainerName == DocumentType.Documents.ToString() &&
                                                            x.BlobFileName == FileName &&
                                                            x.SourceFilePath == $"{BlobStorageConstants.TechSpecFolderName}/{FolderName}"));
        }

        [Fact]
        public void Then_Returns_Expected_Result()
        {
            ActualResult.Should().NotBeNull();
        }
    }
}
