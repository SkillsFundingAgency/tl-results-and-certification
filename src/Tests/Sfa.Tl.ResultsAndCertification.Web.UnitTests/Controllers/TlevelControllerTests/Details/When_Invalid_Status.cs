using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class When_Invalid_Status : TestSetup
    {
        private TLevelConfirmedDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new TLevelConfirmedDetailsViewModel
            {
                IsValid = false,
                Specialisms = new List<string> { "Specialism1", "Specialism2" }
            };
            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(AoUkprn, id).Returns(_mockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
