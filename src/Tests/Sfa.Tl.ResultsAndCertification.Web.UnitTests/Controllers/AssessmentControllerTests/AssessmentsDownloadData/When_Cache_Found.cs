using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using Xunit;

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
            var model = viewResult.Model as AssessmentsDownloadViewModel;

            model.Should().NotBeNull();
            model.CoreAssesmentsDownloadLinkViewModel.BlobUniqueReference.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.BlobUniqueReference);
            model.CoreAssesmentsDownloadLinkViewModel.FileType.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.FileType);
            model.CoreAssesmentsDownloadLinkViewModel.FileSize.Should().Be(AssessmentsDownloadViewModel.CoreAssesmentsDownloadLinkViewModel.FileSize);

            model.SpecialismAsssmentsDownloadLinkViewModel.BlobUniqueReference.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.BlobUniqueReference);
            model.SpecialismAsssmentsDownloadLinkViewModel.FileType.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.FileType);
            model.SpecialismAsssmentsDownloadLinkViewModel.FileSize.Should().Be(AssessmentsDownloadViewModel.SpecialismAsssmentsDownloadLinkViewModel.FileSize);
        }
    }
}
