using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using System.IO;
using System.Text;
using Xunit;
using ResultsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultsDownloadDataLink
{
    public class When_Called_With_Valid_Data_For_Core : TestSetup
    {
        public override void Given()
        {
            Id = Guid.NewGuid().ToString();
            ComponentType = Common.Enum.ComponentType.Core;
            ResultLoader.GetResultsDataFileAsync(AoUkprn, Id.ToGuid(), ComponentType).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for core Results entries")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ResultLoader.Received(1).GetResultsDataFileAsync(AoUkprn, Id.ToGuid(), ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(ResultsContent.ResultsDownloadData.Core_Results_Download_FileName);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
