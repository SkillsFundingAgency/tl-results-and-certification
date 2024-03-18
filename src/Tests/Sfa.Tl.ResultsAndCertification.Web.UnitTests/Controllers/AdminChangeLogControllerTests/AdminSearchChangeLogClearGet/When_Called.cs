using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchChangeLogClearGet
{
    public class When_Called : AdminChangeLogControllerTestBase
    {
        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.AdminSearchChangeLogClearAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminSearchChangeLogViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchChangeLog()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminSearchChangeLog);
        }
    }
}