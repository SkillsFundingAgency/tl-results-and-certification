using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using Xunit;
using IndustryPlacementContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadSuccessful
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadSuccessfulViewModel = new UploadSuccessfulViewModel { Stats = new ViewModel.BulkUploadStatsViewModel { TotalRecordsCount = 10 } };
            CacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(CacheKey).Returns(UploadSuccessfulViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadSuccessfulViewModel;

            model.Should().NotBeNull();

            model.Stats.Should().NotBeNull();
            model.Stats.TotalRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.TotalRecordsCount);
            model.SuccessfulIndustryPlacementsText.Should().Be(string.Format(IndustryPlacementContent.UploadSuccessful.Successfully_Sent_Total_Industry_Placements_Text, UploadSuccessfulViewModel.Stats.TotalRecordsCount));
        }
    }
}
