using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultNoAssessmentEntryViewModel
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            ResultDetailsViewModel = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Now.AddYears(-30),
                TlevelTitle = "Tlevel title",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                PathwayAssessmentSeries = "Summer 2021",
                AppealEndDate = DateTime.Today.AddDays(7),
                PathwayAssessmentId = 11,
                PathwayResultId = 123,
                PathwayResult = "A",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
            };
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Uln.Should().Be(ResultDetailsViewModel.Uln);
            ActualResult.Firstname.Should().Be(ResultDetailsViewModel.Firstname);
            ActualResult.Lastname.Should().Be(ResultDetailsViewModel.Lastname);
            ActualResult.LearnerName.Should().Be($"{ResultDetailsViewModel.Firstname} {ResultDetailsViewModel.Lastname}");
            ActualResult.DateofBirth.Should().Be(ResultDetailsViewModel.DateofBirth);
            ActualResult.ProviderName.Should().Be(ResultDetailsViewModel.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(ResultDetailsViewModel.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(ResultDetailsViewModel.TlevelTitle);
            ActualResult.ProviderDisplayName.Should().Be($"{ResultDetailsViewModel.ProviderName}<br/>({ResultDetailsViewModel.ProviderUkprn})");
        }
    }
}