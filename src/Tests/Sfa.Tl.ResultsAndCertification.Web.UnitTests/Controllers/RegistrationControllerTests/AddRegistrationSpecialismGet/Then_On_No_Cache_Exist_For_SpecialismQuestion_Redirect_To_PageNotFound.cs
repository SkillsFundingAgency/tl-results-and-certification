using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismGet
{
    public class Then_On_No_Cache_Exist_For_SpecialismQuestion_Redirect_To_PageNotFound : When_AddRegistrationSpecialism_Action_Is_Called
    {
        public override void Given()
        {
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(new RegistrationViewModel());
        }

        [Fact]
        public void Then_On_Empty_Cache_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
