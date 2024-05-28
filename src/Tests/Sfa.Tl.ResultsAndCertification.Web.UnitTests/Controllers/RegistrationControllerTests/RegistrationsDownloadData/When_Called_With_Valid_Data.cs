using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationsDownloadData
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RegistrationsDownloadViewModel _viewModel;
        public override void Given()
        {
            _viewModel = new RegistrationsDownloadViewModel
            {
                RegistrationsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 100,
                    FileType = FileType.Csv.ToString()
                }
            };

            CacheService.GetAndRemoveAsync<RegistrationsDownloadViewModel>(CacheKey)
                .Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RegistrationsDownloadViewModel));

            var actualResult = viewResult.Model as RegistrationsDownloadViewModel;
            actualResult.Should().NotBeNull();
            actualResult.RegistrationsDownloadLinkViewModel.Should().NotBeNull();

            actualResult.RegistrationsDownloadLinkViewModel.BlobUniqueReference.Should().Be(_viewModel.RegistrationsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.RegistrationsDownloadLinkViewModel.FileSize.Should().Be(_viewModel.RegistrationsDownloadLinkViewModel.FileSize);
            actualResult.RegistrationsDownloadLinkViewModel.FileType.Should().Be(_viewModel.RegistrationsDownloadLinkViewModel.FileType);

            actualResult.Breadcrumb.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);
            actualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            actualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            actualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RegistrationDashboard);
            actualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Registration_Dashboard);
        }
    }
}
