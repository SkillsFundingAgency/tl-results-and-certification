using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using Xunit;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadSuccessful
{
    public class When_Called_With_One_Registration : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadSuccessfulViewModel = new UploadSuccessfulViewModel { Stats = new ViewModel.BulkUploadStatsViewModel { TotalRecordsCount = 3, NewRecordsCount = 1, AmendedRecordsCount = 1, UnchangedRecordsCount = 1 } };
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
            model.HasMoreThanOneStatsToShow.Should().BeTrue();
            model.SuccessfulRegistrationText.Should().Be(string.Format(RegistrationContent.UploadSuccessful.Successfully_Sent_Total_Registrations_Text, UploadSuccessfulViewModel.Stats.TotalRecordsCount));
            model.NewRegistrationsText.Should().Be(string.Format(RegistrationContent.UploadSuccessful.New_Registrations_Singular_Text, UploadSuccessfulViewModel.Stats.NewRecordsCount));
            model.AmendedRegistrationsText.Should().Be(string.Format(RegistrationContent.UploadSuccessful.Amended_Registrations_Singular_Text, UploadSuccessfulViewModel.Stats.AmendedRecordsCount));
            model.UnchangedRegistrationsText.Should().Be(string.Format(RegistrationContent.UploadSuccessful.Unchanged_Registrations_Singular_Text, UploadSuccessfulViewModel.Stats.UnchangedRecordsCount));
        }
    }
}
