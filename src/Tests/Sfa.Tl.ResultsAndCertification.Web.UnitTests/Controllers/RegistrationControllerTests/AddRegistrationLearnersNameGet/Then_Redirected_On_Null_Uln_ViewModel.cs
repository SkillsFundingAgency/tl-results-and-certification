using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNameGet
{
    public class Then_Redirected_On_Null_Uln_ViewModel : When_AddRegistrationLearnersName_Get_Action_Is_Called
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
