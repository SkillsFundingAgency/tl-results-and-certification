using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthPost
{
    public class Then_On_No_Cache_Redirected_To_PageNotFound : When_AddRegistrationDateofBirth_Post_Action_Is_Called
    {
        public override void Given()
        {
            DateofBirthViewmodel = new DateofBirthViewModel { Day = "01", Month="01", Year = "2020" };
        }

        [Fact]
        public void Then_Valid_DateofBirth_Redirected_ToAddRegistrationLearnerName_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
