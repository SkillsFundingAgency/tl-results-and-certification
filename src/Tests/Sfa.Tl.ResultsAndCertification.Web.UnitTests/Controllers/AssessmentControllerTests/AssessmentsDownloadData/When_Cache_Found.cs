using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentsDownloadData
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            AssessmentsDownloadViewModel = new AssessmentsDownloadViewModel
            {
                CoreAssesmentsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 1.7,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                },
                SpecialismAsssmentsDownloadLinkViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 2.1,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                }
            };
                
            CacheService.GetAndRemoveAsync<AssessmentsDownloadViewModel>(CacheKey).Returns(AssessmentsDownloadViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var actualResult = viewResult.Model as AssessmentsDownloadViewModel;

            actualResult.Should().NotBeNull();
            actualResult.CoreAssesmentsDownloadLinkViewModel.BlobUniqueReference.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.CoreAssesmentsDownloadLinkViewModel.FileType.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.FileType);
            actualResult.CoreAssesmentsDownloadLinkViewModel.FileSize.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.FileSize);

            actualResult.SpecialismAsssmentsDownloadLinkViewModel.BlobUniqueReference.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.BlobUniqueReference);
            actualResult.SpecialismAsssmentsDownloadLinkViewModel.FileType.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.FileType);
            actualResult.SpecialismAsssmentsDownloadLinkViewModel.FileSize.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.FileSize);

            actualResult.Breadcrumb.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            actualResult.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);
            actualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            actualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            actualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.AssessmentDashboard);
            actualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Assessment_Dashboard);
        }
    }
}
