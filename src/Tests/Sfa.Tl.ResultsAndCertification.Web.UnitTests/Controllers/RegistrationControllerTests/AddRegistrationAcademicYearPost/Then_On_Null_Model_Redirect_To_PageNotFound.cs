using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class Then_On_Null_Model_Redirect_To_PageNotFound : When_AddRegistrationAcademicYear_Action_Is_Called
    {
        public override void Given()
        {
            SelectAcademicYearViewModel = null;
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(new RegistrationViewModel());
        }

        [Fact]
        public void Then_On_Null_ViewModel_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
