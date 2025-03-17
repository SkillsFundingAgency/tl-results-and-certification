using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminSelectAwardingOrganisationClearGet
{
    public class When_Called : AdminAwardingOrganisationControllerBaseTest
    {
        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminSelectAwardingOrganisationClearAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminSelectAwardingOrganisationViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchChangeLog()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminSelectAwardingOrganisation);
        }
    }
}