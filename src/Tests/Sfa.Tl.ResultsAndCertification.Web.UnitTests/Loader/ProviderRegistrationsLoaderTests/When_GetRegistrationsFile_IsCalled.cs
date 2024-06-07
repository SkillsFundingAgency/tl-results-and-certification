using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Content.ProviderRegistrations;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderRegistrationsLoaderTests
{
    public class When_GetRegistrationsFile_IsCalled : ProviderRegistrationsLoaderBaseTest
    {
        private const long ProviderUkprn = 1;
        private readonly Guid BlobUniqueReference = new("b7a8d6e1-3c0e-4f1b-9c5a-2e1e1b4c8e6d");

        private readonly Stream _fileStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });

        private FileStreamResult _actualResult;

        public override void Given()
        {
            BlobStorageService.DownloadFileAsync(Arg.Is<BlobStorageData>(b =>
                b.ContainerName == DocumentType.DataExports.ToString()
                && b.BlobFileName == $"{BlobUniqueReference}.{FileType.Csv}"
                && b.SourceFilePath == $"{ProviderUkprn}/{DataExportType.ProviderRegistrations}"))
            .Returns(_fileStream);
        }

        public override async Task When()
        {
            _actualResult = await Loader.GetRegistrationsFileAsync(ProviderUkprn, BlobUniqueReference);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.FileDownloadName.Should().Be(DownloadRegistrationsDataFor.Registrations_Data_Report_File_Name_Text);
            _actualResult.FileStream.Should().BeSameAs(_fileStream);
            _actualResult.ContentType.Should().Be("text/csv");
        }
    }
}