using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordPost
{
    public class When_IsLearnerRegistered_IsFalse : TestSetup
    {
        private readonly long uln = 123456789;
        private FindLearnerRecord mockResult;

        public override void Given()
        {
            SearchLearnerRecordViewModel = new SearchLearnerRecordViewModel { SearchUln = uln.ToString() };
            mockResult = new FindLearnerRecord { IsLearnerRegistered = false };
            TrainingProviderLoader.FindLearnerRecordAsync(ProviderUkprn, SearchLearnerRecordViewModel.SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(ProviderUkprn, uln);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<SearchLearnerRecordViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.SearchLearnerRecordNotFound);
        }
    }
}
