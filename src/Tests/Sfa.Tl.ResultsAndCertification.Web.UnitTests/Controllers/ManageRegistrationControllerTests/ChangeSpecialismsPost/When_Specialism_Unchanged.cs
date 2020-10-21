using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsPost
{
    public class When_Specialism_Unchanged : TestSetup
    {
        private ManageRegistrationResponse mockResponse = null;

        public override void Given()
        {
            mockResponse = new ManageRegistrationResponse
            {
                IsModified = false,
            };

            RegistrationLoader.ProcessSpecialismChangeAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationDetails);
        }
    }
}
