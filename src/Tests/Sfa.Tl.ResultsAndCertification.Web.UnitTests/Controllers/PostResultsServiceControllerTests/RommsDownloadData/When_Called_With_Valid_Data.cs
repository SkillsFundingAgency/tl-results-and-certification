using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.RommsDownloadData
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RommsDownloadViewModel _viewModel;
        public override void Given()
        {
            _viewModel = new RommsDownloadViewModel
            {
                RommsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 100,
                    FileType = FileType.Csv.ToString()
                }
            };

            CacheService.GetAndRemoveAsync<RommsDownloadViewModel>(CacheKey)
                .Returns(_viewModel);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();

            var actualResult = Result.ShouldBeViewResult<RommsDownloadViewModel>();
            actualResult.Should().NotBeNull();
            actualResult.RommsDownloadLinkViewModel.Should().NotBeNull();

            actualResult.RommsDownloadLinkViewModel.BlobUniqueReference.Should().Be(_viewModel.RommsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.RommsDownloadLinkViewModel.FileSize.Should().Be(_viewModel.RommsDownloadLinkViewModel.FileSize);
            actualResult.RommsDownloadLinkViewModel.FileType.Should().Be(_viewModel.RommsDownloadLinkViewModel.FileType);

            actualResult.Breadcrumb.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            actualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            actualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);

            actualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultReviewsAndAppeals);
            actualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.ResultReviewsAndAppeals);

            actualResult.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
            actualResult.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Download_Romms_Data);
        }
    }
}