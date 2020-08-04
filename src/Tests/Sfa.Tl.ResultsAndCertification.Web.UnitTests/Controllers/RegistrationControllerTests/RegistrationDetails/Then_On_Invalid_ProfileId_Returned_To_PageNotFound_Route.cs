using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationDetails
{
    public class Then_On_Invalid_ProfileId_Returned_To_PageNotFound_Route : When_RegistrationDetails_Action_Is_Called
    {
        private RegistrationDetailsViewModel mockresult = null;
        public override void Given()
        {

            //var mockresult = new RegistrationDetailsViewModel
            //{
            //    ProfileId = 1,
            //    Uln = 1234567890,
            //    Name = "Test",
            //    DateofBirth = DateTime.UtcNow,
            //    ProviderDisplayName = "Test Provider (1234567)",
            //    PathwayDisplayName = "Pathway (7654321)",
            //    SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
            //    AcademicYear = 2020
            //};

            RegistrationLoader.GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_To_Return_PageNotFound_Page()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
