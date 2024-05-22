using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationClearGet
{
    public class When_Called : SearchRegistrationControllerTestBase
    {
        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;
        private IActionResult _result;

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationClearAsync(_searchRegitrationType);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<SearchRegistrationViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.SearchRegistration, (Constants.Type, _searchRegitrationType));
        }
    }
}