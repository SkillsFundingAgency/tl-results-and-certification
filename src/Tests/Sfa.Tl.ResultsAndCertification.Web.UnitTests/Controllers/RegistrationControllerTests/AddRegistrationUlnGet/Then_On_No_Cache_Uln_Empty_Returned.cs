using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnGet
{
    public class Then_On_No_Cache_Uln_Empty_Returned : When_AddRegistrationUln_Action_Is_Called
    {
        public override void Given() {}

        [Fact]
        public void Then_ViewModel_Returns_Cached_UlnViewModel()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UlnViewModel));

            var model = viewResult.Model as UlnViewModel;
            model.Should().NotBeNull();
            model.Uln.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RegistrationDashboard);
        }
    }
}
