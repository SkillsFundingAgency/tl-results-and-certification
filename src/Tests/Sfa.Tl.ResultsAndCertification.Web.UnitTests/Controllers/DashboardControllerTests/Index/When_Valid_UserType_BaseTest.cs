using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public abstract class When_Valid_UserType_BaseTest : TestSetup
    {
        public void Given(LoginUserType loginUserType)
        {
            var httpContext = new ClaimsIdentityBuilder<DashboardController>(Controller)
                .Add(CustomClaimTypes.HasAccessToService, "true")
                .Add(CustomClaimTypes.LoginUserType, ((int)loginUserType).ToString())
                .Build().HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public void Then_Expected_Result(LoginUserType loginUserType)
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();

            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as DashboardViewModel;
            model.Should().NotBeNull();

            model.LoginUserType.Should().Be(loginUserType);
        }
    }
}