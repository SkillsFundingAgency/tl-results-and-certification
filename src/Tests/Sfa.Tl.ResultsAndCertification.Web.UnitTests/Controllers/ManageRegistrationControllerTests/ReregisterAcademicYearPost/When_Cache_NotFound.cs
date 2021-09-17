using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearPost
{
    public class When_Cache_NotFound : TestSetup
    {
        public override void Given()
        {
            ReregisterViewModel reregisterViewModel = null;
            AcademicYearViewModel = new ReregisterAcademicYearViewModel();
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(reregisterViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
