using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests
{
    // TODO: To be refactored to use NSubstitute. 
    public class AccountControllerTests
    {
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly Mock<ILogger<AccountController>> mockLogger;
        
        private readonly AccountController controller;
        private readonly ControllerContext controllerContext;
        
        public AccountControllerTests()
        {
            mockHttpContext = new Mock<HttpContext>();
            mockLogger = new Mock<ILogger<AccountController>>();

            mockHttpContext.Setup(x => x.User.Identity.Name).Returns("AuthUser");

            controllerContext = new ControllerContext();
            controllerContext.HttpContext = mockHttpContext.Object;

            controller = new AccountController(mockLogger.Object);
            controller.ControllerContext = controllerContext;
        }

        [Fact]
        public void PostSignIn_WhenUserAuthenticated_ThenRedirectedToDashboard()
        {
            // Given
            mockHttpContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);
            mockHttpContext.Setup(x => x.User.Claims).Returns(new List<Claim> { new Claim(CustomClaimTypes.HasAccessToService, "true") });

            // When
            var result = controller.PostSignIn();

            // Then
            Assert.Same((result as RedirectToActionResult).ActionName, nameof(DashboardController.Index));
            Assert.Same((result as RedirectToActionResult).ControllerName, Constants.DashboardController);
        }


        [Fact]
        public void PostSignIn_WhenUserAuthenticatedWithNoAccessToService_ThenRedirectedToServiceAccessDenied()
        {
            // Given
            mockHttpContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(true);
            mockHttpContext.Setup(x => x.User.Claims).Returns(new List<Claim>());

            // When
            var result = controller.PostSignIn();

            // Then
            Assert.Same((result as RedirectToActionResult).ActionName, nameof(ErrorController.ServiceAccessDenied));
            Assert.Same((result as RedirectToActionResult).ControllerName, Constants.ErrorController);
        }

        [Fact]
        public void PostSignIn_WhenUserNotAuthenticated_ThenRedirectedToHome()
        {
            // Given
            mockHttpContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            // When
            var result = controller.PostSignIn();

            // Then
            Assert.Same((result as RedirectToActionResult).ActionName, nameof(HomeController.Index));
            Assert.Same((result as RedirectToActionResult).ControllerName, Constants.HomeController);
        }
    }
}
