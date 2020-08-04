using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class Then_On_Uln_NotFound_Redirect_To_SearchRegistrationNotFound_Route : When_SearchRegistration_Post_Action_Is_Called
    {
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = SearchUln };
        }

        [Fact]
        public void Then_AddRegistrationAsync_Is_Called()
        {
            RegistrationLoader.Received().FindUlnAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationNotFound_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SearchRegistrationNotFound);
        }
    }
}
