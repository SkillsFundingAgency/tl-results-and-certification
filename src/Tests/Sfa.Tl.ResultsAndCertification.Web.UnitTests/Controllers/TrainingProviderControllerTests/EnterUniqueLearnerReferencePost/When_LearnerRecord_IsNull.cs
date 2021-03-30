using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_LearnerRecord_IsNull : TestSetup
    {
        private readonly long uln = 123456789;
        private readonly FindLearnerRecord mockResult = null;
        private AddLearnerRecordViewModel mockCacheViewModel;

        public override void Given()
        {
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = uln.ToString() };
            TrainingProviderLoader.FindLearnerRecordAsync(ProviderUkprn, uln).Returns(mockResult);

            mockCacheViewModel = new AddLearnerRecordViewModel { LearnerRecord = mockResult };
            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(mockCacheViewModel);
        }

        [Fact]
        public void Then_Redirected_To_EnterUniqueLearnerNumberNotFound()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumberNotFound);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(ProviderUkprn, uln);
            
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<AddLearnerRecordViewModel>());
        }
    }
}