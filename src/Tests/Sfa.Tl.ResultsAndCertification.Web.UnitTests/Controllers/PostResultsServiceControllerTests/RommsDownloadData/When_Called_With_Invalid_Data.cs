using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.RommsDownloadData
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private readonly RommsDownloadViewModel _viewModel = null;
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<RommsDownloadViewModel>(CacheKey)
                .Returns(_viewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
