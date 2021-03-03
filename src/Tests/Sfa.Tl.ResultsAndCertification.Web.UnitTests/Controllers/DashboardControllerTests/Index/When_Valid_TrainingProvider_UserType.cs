using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public class When_Valid_TrainingProvider_UserType : TestSetup
    {
        public override void Given()
        {
            var httpContext = new ClaimsIdentityBuilder<DashboardController>(Controller)
                .Add(CustomClaimTypes.HasAccessToService, "true")
                .Add(CustomClaimTypes.LoginUserType, ((int)LoginUserType.TrainingProvider).ToString())
                .Build().HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as DashboardViewModel;
            model.Should().NotBeNull();
            model.IsAoUser.Should().BeFalse();
            model.IsTrainingProviderUser.Should().BeTrue();
        }
    }
}