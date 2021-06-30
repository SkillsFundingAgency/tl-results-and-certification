using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberPost
{
    public class When_FindSoaLearnerRecord_IsNull : TestSetup
    {
        private FindSoaLearnerRecord _soaLearnerRecord = null;

        public override void Given()
        {
            ViewModel = new RequestSoaUniqueLearnerNumberViewModel { SearchUln = "1234567891" };
            _soaLearnerRecord = null;
            StatementOfAchievementLoader.FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong()).Returns(_soaLearnerRecord);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<RequestSoaUlnNotFoundViewModel>(x =>
                    x.Uln == ViewModel.SearchUln), 
                    Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_RequestSoaUlnNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RequestSoaUlnNotFound);
        }
    }
}
