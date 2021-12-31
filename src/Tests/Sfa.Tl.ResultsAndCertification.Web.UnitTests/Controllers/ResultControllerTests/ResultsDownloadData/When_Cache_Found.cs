using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultsDownloadData
{
    public class When_Cache_Found : TestSetup
    {
        private ResultsDownloadViewModel _resultsDownloadViewModel;

        public override void Given()
        {
            _resultsDownloadViewModel = new ResultsDownloadViewModel
            {
                CoreResultsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 1.7,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                },
                SpecialismResultsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 2.1,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                }
            };

            CacheService.GetAndRemoveAsync<ResultsDownloadViewModel>(CacheKey).Returns(_resultsDownloadViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var actualResult = viewResult.Model as ResultsDownloadViewModel;

            actualResult.Should().NotBeNull();
            actualResult.CoreResultsDownloadLinkViewModel.BlobUniqueReference.Should().Be(_resultsDownloadViewModel.CoreResultsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.CoreResultsDownloadLinkViewModel.FileType.Should().Be(_resultsDownloadViewModel.CoreResultsDownloadLinkViewModel.FileType);
            actualResult.CoreResultsDownloadLinkViewModel.FileSize.Should().Be(_resultsDownloadViewModel.CoreResultsDownloadLinkViewModel.FileSize);

            actualResult.SpecialismResultsDownloadLinkViewModel.BlobUniqueReference.Should().Be(_resultsDownloadViewModel.SpecialismResultsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.SpecialismResultsDownloadLinkViewModel.FileType.Should().Be(_resultsDownloadViewModel.SpecialismResultsDownloadLinkViewModel.FileType);
            actualResult.SpecialismResultsDownloadLinkViewModel.FileSize.Should().Be(_resultsDownloadViewModel.SpecialismResultsDownloadLinkViewModel.FileSize);

            actualResult.Breadcrumb.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);
            actualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            actualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            actualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            actualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Result_Dashboard);
        }
    }
}
