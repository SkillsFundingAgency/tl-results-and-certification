using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_AppealGradeAfterDeadlineConfirmViewModel : TestSetup
    {
        private Models.Contracts.PostResultsService.PrsLearnerDetails _expectedApiResult;
        protected AppealGradeAfterDeadlineConfirmViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 2;
            _expectedApiResult = new Models.Contracts.PostResultsService.PrsLearnerDetails
            {
                ProfileId = ProfileId,
                Uln = 123456789,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 54678945,
                TlevelTitle = "Title",
                Status = RegistrationPathwayStatus.Active,

                PathwayAssessmentId = AssessmentId,
                PathwayAssessmentSeries = "Summer 2021",
                PathwayCode = "12345678",
                PathwayName = "Childcare Education",
                PathwayGrade = "A*",
                PathwayResultId = 77,
                PathwayPrsStatus = null,
                PathwayGradeLastUpdatedBy = "Barsley User",
                PathwayGradeLastUpdatedOn = DateTime.Today,
            };

            InternalApiClient.GetPrsLearnerDetailsAsync(AoUkprn, ProfileId, AssessmentId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.PathwayAssessmentId.Should().Be(_expectedApiResult.PathwayAssessmentId);
            ActualResult.AppealEndDate.Should().Be(_expectedApiResult.AppealEndDate);
            ActualResult.PathwayPrsStatus.Should().Be(_expectedApiResult.PathwayPrsStatus);
            ActualResult.Status.Should().Be(_expectedApiResult.Status);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be($"{_expectedApiResult.Firstname} {_expectedApiResult.Lastname}");
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.PathwayDisplayName.Should().Be($"{_expectedApiResult.PathwayName}<br/>({_expectedApiResult.PathwayCode})");
            ActualResult.PathwayGrade.Should().Be(_expectedApiResult.PathwayGrade);
            ActualResult.PathwayAssessmentSeries.Should().Be(_expectedApiResult.PathwayAssessmentSeries);
            ActualResult.IsThisGradeBeingAppealed.Should().BeNull();
            ActualResult.IsValid.Should().BeTrue();

            ActualResult.BackLink.Should().NotBeNull();
            ActualResult.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealGradeAfterDeadline);
            ActualResult.BackLink.RouteAttributes.Count.Should().Be(2);
            ActualResult.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            ActualResult.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
        }
    }
}
