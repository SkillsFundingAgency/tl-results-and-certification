using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeSpecialismViewModel mockChangeSpecialismViewModel = null;
        private PathwaySpecialismsViewModel mockPathwaySpecialismsViewModel = null;
        public override void Given()
        {

            mockPathwaySpecialismsViewModel = new PathwaySpecialismsViewModel
            {
                Specialisms = new List<SpecialismDetailsViewModel>
                {
                    new SpecialismDetailsViewModel { Id = 1, Code = "11111" },
                    new SpecialismDetailsViewModel { Id = 2, Code = "77777" },
                    new SpecialismDetailsViewModel { Id = 3, Code = "99999" },
                    new SpecialismDetailsViewModel { Id = 4, Code = "33333" },
                }
            };
            mockChangeSpecialismViewModel = new ChangeSpecialismViewModel
            {
                ProfileId = ProfileId,
                CoreCode = "12345678",
                PathwaySpecialisms = mockPathwaySpecialismsViewModel,
                SpecialismCodes = new List<string> { "77777", "99999" }
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(AoUkprn, ProfileId).Returns(mockChangeSpecialismViewModel);
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, mockChangeSpecialismViewModel.CoreCode).Returns(mockPathwaySpecialismsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeSpecialismViewModel));

            var model = viewResult.Model as ChangeSpecialismViewModel;
            model.Should().NotBeNull();
            model.CoreCode.Should().Be(mockChangeSpecialismViewModel.CoreCode);
            model.ProfileId.Should().Be(mockChangeSpecialismViewModel.ProfileId);
            model.SpecialismCodes.Should().NotBeNullOrEmpty();
            model.SpecialismCodes.Count().Should().Be(2);
            model.SpecialismCodes.ToList().ForEach(x => { mockChangeSpecialismViewModel.SpecialismCodes.Contains(x); });

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ChangeRegistrationSpecialismQuestion);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockChangeSpecialismViewModel.ProfileId.ToString());
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            RegistrationLoader.Received(1).GetRegistrationProfileAsync<ChangeSpecialismViewModel>(AoUkprn, ProfileId);
            RegistrationLoader.Received(1).GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, mockChangeSpecialismViewModel.CoreCode);
        }
    }
}
