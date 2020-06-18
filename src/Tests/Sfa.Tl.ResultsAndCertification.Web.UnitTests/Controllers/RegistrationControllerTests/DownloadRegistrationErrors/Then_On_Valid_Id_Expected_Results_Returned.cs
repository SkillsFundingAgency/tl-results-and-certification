using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Xunit;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DownloadRegistrationErrors
{
    public class Then_On_Valid_Id_Expected_Results_Returned : When_DownloadRegistrationErrors_Is_Called
    {
        public override void Given()
        {
            Id = BlobUniqueReference.ToString();
        }

        [Fact]
        public void Then_GetRegistrationValidationErrorsFileAsync_Method_Is_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationValidationErrorsFileAsync(Ukprn, Id.ToGuid());
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var viewResult = Result.Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(RegistrationContent.UploadUnsuccessful.Registrations_Error_Report_File_Name_Text);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
