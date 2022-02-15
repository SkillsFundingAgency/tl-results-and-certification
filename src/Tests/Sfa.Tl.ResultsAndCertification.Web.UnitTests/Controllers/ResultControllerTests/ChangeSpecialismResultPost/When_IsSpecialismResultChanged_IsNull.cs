using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultPost
{
    public class When_IsSpecialismResultChanged_IsNull : TestSetup
    {
        private readonly bool? _mockResult = null;
        public override void Given()
        {
            ResultLoader.IsSpecialismResultChangedAsync(AoUkprn, Arg.Any<ManageSpecialismResultViewModel>()).Returns(_mockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
