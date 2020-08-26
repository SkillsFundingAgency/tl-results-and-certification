using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class Then_On_Valid_Uln_Redirected_To_Route_LearnerName : When_AddRegistrationUln_Action_Is_Called
    {
        public override void Given()
        {
            UlnViewModel = new UlnViewModel { Uln = "1234567890" };
        }

        [Fact]
        public void Then_Uln_Cache_Is_Synchronised()
        {
            CacheService.Received(1).GetAsync<RegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<RegistrationViewModel>());
        }

        [Fact]
        public void Then_Valid_Uln_Redirected_ToAddRegistrationLearnerName_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationLearnersName);
        }

        [Fact]
        public void Then_FindUlnAsync_Method_Is_Called()
        {
            RegistrationLoader.Received(1).FindUlnAsync(Arg.Any<long>(), UlnViewModel.Uln.ToLong());
        }
    }
}
