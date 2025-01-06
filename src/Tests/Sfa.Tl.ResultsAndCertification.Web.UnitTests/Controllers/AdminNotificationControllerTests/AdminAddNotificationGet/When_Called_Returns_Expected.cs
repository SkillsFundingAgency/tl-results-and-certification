using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminAddNotificationGet
{
    public class When_Called_Returns_Expected : AdminNotificationControllerBaseTest
    {
        public override void Given()
        {
        }

        private IActionResult _result;

        public override Task When()
        {
            _result = Controller.AdminAddNotificationAsync();
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var viewModel = _result.ShouldBeViewResult<AdminAddNotificationViewModel>();

            viewModel.Title.Should().BeNull();
            viewModel.Content.Should().BeNull();
            viewModel.Target.Should().Be(NotificationTarget.NotSpecified);
            viewModel.StartDay.Should().BeNull();
            viewModel.StartDay.Should().BeNull();
            viewModel.StartMonth.Should().BeNull();
            viewModel.StartYear.Should().BeNull();
            viewModel.EndDay.Should().BeNull();
            viewModel.EndMonth.Should().BeNull();
            viewModel.EndYear.Should().BeNull();

            viewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminFindNotification);
            viewModel.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}