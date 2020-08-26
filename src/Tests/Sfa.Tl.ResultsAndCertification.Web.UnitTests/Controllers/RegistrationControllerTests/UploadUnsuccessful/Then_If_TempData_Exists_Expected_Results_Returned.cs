using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadUnsuccessful
{
    public class Then_If_TempData_Exists_Expected_Results_Returned : When_UploadUnsuccessful_Is_Called
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadUnsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = BlobUniqueReference, FileSize = 1.7, FileType = FileType.Csv.ToString().ToUpperInvariant() };
            CacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(Arg.Any<string>()).Returns(UploadUnsuccessfulViewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadUnsuccessfulViewModel;

            model.Should().NotBeNull();
            model.BlobUniqueReference.Should().Be(UploadUnsuccessfulViewModel.BlobUniqueReference);
            model.FileSize.Should().Be(UploadUnsuccessfulViewModel.FileSize);
            model.FileType.Should().Be(UploadUnsuccessfulViewModel.FileType);
        }
    }
}
