using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsGet
{
    public class When_None_PreSelected : TestSetup
    {
        private ChangeSpecialismViewModel mockChangeSpecialismViewModel = null;
        private PathwaySpecialismsViewModel mockPathwaySpecialismsViewModel = null;
        public override void Given()
        {

            mockPathwaySpecialismsViewModel = new PathwaySpecialismsViewModel
            {
                Specialisms = new List<SpecialismDetailsViewModel>()
            };

            mockChangeSpecialismViewModel = new ChangeSpecialismViewModel
            {
                CoreCode = "12345678",
                SpecialismCodes = new List<string>()
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(AoUkprn, ProfileId).Returns(mockChangeSpecialismViewModel);
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, mockChangeSpecialismViewModel.CoreCode).Returns(mockPathwaySpecialismsViewModel);
        }

        [Fact]
        public void Then_Expected_BackLink_Route_Set()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeSpecialismViewModel));

            var model = viewResult.Model as ChangeSpecialismViewModel;
            model.Should().NotBeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockChangeSpecialismViewModel.ProfileId.ToString());
        }
    }
}
