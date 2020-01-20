using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers
{
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
            mockHttpContext.Setup(x => x.User.Claims).Returns(new List<Claim> { new Claim("HasAccessToService", "true") });

            // When
            var result = controller.PostSignIn();

            // Then
            Assert.Same((result as RedirectToActionResult).ActionName, "Index");
            Assert.Same((result as RedirectToActionResult).ControllerName, "Dashboard");
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
            Assert.Same((result as RedirectToActionResult).ActionName, "ServiceAccessDenied");
            Assert.Same((result as RedirectToActionResult).ControllerName, "Error");
        }

        [Fact]
        public void PostSignIn_WhenUserNotAuthenticated_ThenRedirectedToHome()
        {
            // Given
            mockHttpContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            // When
            var result = controller.PostSignIn();

            // Then
            Assert.Same((result as RedirectToActionResult).ActionName, "Index");
            Assert.Same((result as RedirectToActionResult).ControllerName, "Home");
        }
    }
}
