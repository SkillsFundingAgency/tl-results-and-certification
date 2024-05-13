using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public class When_FindPrsLearnerRecord_IsNull : TestSetup
    {
        private readonly FindPrsLearnerRecord _findPrsLearner = null;

        public override void Given()
        {
            ViewModel = new PrsSearchLearnerViewModel { SearchUln = "1234567891" };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong()).Returns(_findPrsLearner);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsUlnNotFoundViewModel>(x =>
                    x.Uln == ViewModel.SearchUln),
                    Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PostResultServiceUlnNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsUlnNotFound);
        }
    }
}
