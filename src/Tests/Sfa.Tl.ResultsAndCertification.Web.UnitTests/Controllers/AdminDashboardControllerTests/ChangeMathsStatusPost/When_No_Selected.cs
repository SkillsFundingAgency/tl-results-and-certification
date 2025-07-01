using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeMathsStatusPost
{
    public class When_No_Selected : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;
        private const string PathwayIdRouteName = Constants.PathwayId;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotSpecified, SubjectStatus.NotAchieved);
        }

        public async override Task When()
        {
            Result = await Controller.AdminChangeMathsStatusAsync(ViewModel);
        }

        [Fact]
        public void Then_No_Relevant_Methods_AreCalled()
        {
            AdminDashboardLoader.DidNotReceive().GetAdminLearnerRecordAsync<AdminChangeMathsResultsViewModel>(Arg.Any<int>());

            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminChangeMathsResultsViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_BackLink()
        {
            var redirectToRouteResult = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            redirectToRouteResult.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
            redirectToRouteResult.RouteValues.Should().NotBeNull();

            redirectToRouteResult.RouteValues.Should().ContainKey(PathwayIdRouteName);

            redirectToRouteResult.RouteValues[PathwayIdRouteName].Should().Be(ExpectedRegistrationPathwayId.ToString());
        }
    }
}