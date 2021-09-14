using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Xunit;
using ResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.DownloadResultErrors
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            Id = BlobUniqueReference.ToString();
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ResultLoader.Received(1).GetResultValidationErrorsFileAsync(Ukprn, Id.ToGuid());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(ResultContent.UploadUnsuccessful.Result_Error_Report_File_Name);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
