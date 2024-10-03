using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using RommContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.UploadRommsSuccessful
{
    public class When_Called_With_Amended_Romms : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadSuccessfulViewModel = new UploadSuccessfulViewModel { Stats = new ViewModel.BulkUploadStatsViewModel { TotalRecordsCount = 10, AmendedRecordsCount = 10 } };
            CacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(Arg.Any<string>()).Returns(UploadSuccessfulViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadSuccessfulViewModel;

            model.Should().NotBeNull();

            model.Stats.Should().NotBeNull();
            model.Stats.TotalRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.TotalRecordsCount);
            model.Stats.NewRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.NewRecordsCount);
            model.Stats.AmendedRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.AmendedRecordsCount);
            model.Stats.UnchangedRecordsCount.Should().Be(UploadSuccessfulViewModel.Stats.UnchangedRecordsCount);
            model.HasMoreThanOneStatsToShow.Should().BeFalse();
            model.HasNewRomms.Should().BeFalse();
            model.HasAmendedRomms.Should().BeTrue();
            model.HasUnchangedRomms.Should().BeFalse();
            model.SuccessfulRommText.Should().Be(string.Format(RommContent.UploadRommsSuccessful.Successfully_Sent_Amended_Romms_Text, UploadSuccessfulViewModel.Stats.AmendedRecordsCount));
        }
    }
}
