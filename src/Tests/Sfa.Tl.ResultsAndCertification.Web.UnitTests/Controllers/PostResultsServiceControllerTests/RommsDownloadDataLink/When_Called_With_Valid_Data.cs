using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.IO;
using System.Text;
using Xunit;
using RommsDownloadContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.PostResultsService.RommsDownloadDataLink.RommsDownloadDataLink
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            Id = BlobUniqueReference.ToString();
            PostResultsServiceLoader.GetRommsDataFileAsync(Ukprn, BlobUniqueReference).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for validation errors")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            PostResultsServiceLoader.Received(1).GetRommsDataFileAsync(Ukprn, Id.ToGuid());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(RommsDownloadContent.RommsDownloadData.Romms_Data_Report_File_Name_Text);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
