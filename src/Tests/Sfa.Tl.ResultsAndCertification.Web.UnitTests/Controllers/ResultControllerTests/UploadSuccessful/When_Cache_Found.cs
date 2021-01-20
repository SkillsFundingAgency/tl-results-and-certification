using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using Xunit;
using ResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadSuccessful
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
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received().GetAndRemoveAsync<UploadSuccessfulViewModel>(Arg.Any<string>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadSuccessfulViewModel;

            model.Should().NotBeNull();

            model.Stats.Should().NotBeNull();
            model.Stats.TotalRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.TotalRecordsCount);
            model.SuccessfulResultText.Should().Be(string.Format(ResultContent.UploadSuccessful.Successfully_Sent_Total_Results_Text, UploadSuccessfulViewModel.Stats.TotalRecordsCount));
        }
    }
}
