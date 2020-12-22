using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsPost
{
    public class When_Specialism_Change_Sucess : TestSetup
    {
        private ManageRegistrationResponse mockResponse = null;

        public override void Given()
        {
            ViewModel = new ChangeSpecialismViewModel
            {
                SelectedSpecialismCode = "SPL12345",

                PathwaySpecialisms = new PathwaySpecialismsViewModel
                {
                    Specialisms = new List<SpecialismDetailsViewModel>
                    {
                        new SpecialismDetailsViewModel { Id = 11, Code = "SPL12345" },
                        new SpecialismDetailsViewModel { Id = 22, Code = "SPL12346" },
                        new SpecialismDetailsViewModel { Id = 33, Code = "SPL12347" },
                    }
                }
            };
            mockResponse = new ManageRegistrationResponse
            {
                IsModified = true,
                IsSuccess = true
            };

            RegistrationLoader.ProcessSpecialismChangeAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_ChangeRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationConfirmation);
        }

        [Fact]
        public void Then_ProcessSpecialismChangeAsync_IsCalled()
        {
            RegistrationLoader.Received(1).ProcessSpecialismChangeAsync(AoUkprn,
                Arg.Is<ChangeSpecialismViewModel>(x => x.PathwaySpecialisms.Specialisms.SingleOrDefault(s => s.IsSelected).Id == 11));
        }
    }
}
