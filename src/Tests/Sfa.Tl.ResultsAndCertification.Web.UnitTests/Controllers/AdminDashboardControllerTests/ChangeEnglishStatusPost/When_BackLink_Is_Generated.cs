using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeEnglishStatusPost
{
    public class When_BackLink_Is_Generated : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;
        private const string PathwayIdRouteName = Constants.PathwayId;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotSpecified, SubjectStatus.NotAchieved);
        }

        public async override Task When()
        {
            Result = await Controller.AdminChangeEnglishStatusAsync(ViewModel);
        }

        [Fact]
        public void Then_No_Relevant_Methods_AreCalled()
        {
            AdminDashboardLoader.DidNotReceive().GetAdminLearnerRecordAsync<AdminChangeEnglishStatusViewModel>(Arg.Any<int>());

            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminChangeEnglishStatusViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_BackLink_Should_Generate_Correctly()
        {
            var redirectToRouteResult = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            redirectToRouteResult.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
            redirectToRouteResult.RouteValues.Should().NotBeNull();

            redirectToRouteResult.RouteValues.Should().ContainKey(PathwayIdRouteName);

            redirectToRouteResult.RouteValues[PathwayIdRouteName].Should().Be(ExpectedRegistrationPathwayId.ToString());
        }
    }
}