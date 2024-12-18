using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminNotificationDetailsGet
{
    public class When_NotificationDetails_Not_Found : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1; 

        public override void Given()
        {
            AdminNotificationLoader.GetNotificationDetailsViewModel(NotificationId).Returns(null as AdminNotificationDetailsViewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminNotificationDetailsAsync(NotificationId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.GetNotificationDetailsViewModel(NotificationId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}