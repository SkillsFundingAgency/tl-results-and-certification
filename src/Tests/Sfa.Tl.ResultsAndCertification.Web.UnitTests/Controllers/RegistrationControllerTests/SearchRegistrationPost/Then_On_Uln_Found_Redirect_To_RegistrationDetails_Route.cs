using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class Then_On_Uln_Found_Redirect_To_RegistrationDetails_Route : When_SearchRegistration_Post_Action_Is_Called
    {
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = SearchUln };
            var mockResult = new UlnNotFoundViewModel { IsActive = true, Uln = SearchUln };
            RegistrationLoader.FindUlnAsync(AoUkprn, SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_FindUlnAsync_Is_Called()
        {
            RegistrationLoader.Received().FindUlnAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_RegistrationDetails_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationDetails);
        }
    }
}
