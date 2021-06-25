using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetailsTests
{
    public class When_Called_With_PrsLearnerDetailsViewModel : TestSetup
    {
        private Models.Contracts.PostResultsService.PrsLearnerDetails _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = new Models.Contracts.PostResultsService.PrsLearnerDetails
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 54678945,
                TlevelTitle = "Title",
                Status = RegistrationPathwayStatus.Active,

                PathwayAssessmentId = 99,
                PathwayAssessmentSeries = "Summer 2021",
                PathwayCode = "12345678",
                PathwayName = "Childcare Education",
                PathwayGrade = "A*",
                PathwayResultId = 77,
                PathwayGradeLastUpdatedBy = "Barsley User",
                PathwayGradeLastUpdatedOn = DateTime.Today
            };

            InternalApiClient.GetPrsLearnerDetailsAsync(AoUkprn, ProfileId, AssessmentId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.Status.Should().Be(_expectedApiResult.Status);

            ActualResult.PathwayAssessmentId.Should().Be(_expectedApiResult.PathwayAssessmentId);
            ActualResult.PathwayAssessmentSeries.Should().Be(_expectedApiResult.PathwayAssessmentSeries);
            ActualResult.PathwayTitle.Should().Be($"{_expectedApiResult.PathwayName} ({_expectedApiResult.PathwayCode})");
            ActualResult.PathwayGrade.Should().Be(_expectedApiResult.PathwayGrade);
            ActualResult.PathwayResultId.Should().Be(_expectedApiResult.PathwayResultId);
            ActualResult.PathwayGradeLastUpdatedBy.Should().Be(_expectedApiResult.PathwayGradeLastUpdatedBy);
            ActualResult.PathwayGradeLastUpdatedOn.Should().Be(_expectedApiResult.PathwayGradeLastUpdatedOn.ToDobFormat());
        }
    }
}