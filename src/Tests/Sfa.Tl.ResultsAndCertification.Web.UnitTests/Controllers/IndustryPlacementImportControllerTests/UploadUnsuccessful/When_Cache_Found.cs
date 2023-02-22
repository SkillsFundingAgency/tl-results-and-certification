using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadUnsuccessful
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadUnsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = BlobUniqueReference, FileSize = 1.7, FileType = FileType.Csv.ToString().ToUpperInvariant() };
            CacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(Arg.Any<string>()).Returns(UploadUnsuccessfulViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadUnsuccessfulViewModel;

            model.Should().NotBeNull();
            model.BlobUniqueReference.Should().Be(UploadUnsuccessfulViewModel.BlobUniqueReference);
            model.FileSize.Should().Be(UploadUnsuccessfulViewModel.FileSize);
            model.FileType.Should().Be(UploadUnsuccessfulViewModel.FileType);

            // Breadcrumbs
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(1);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
        }
    }
}
