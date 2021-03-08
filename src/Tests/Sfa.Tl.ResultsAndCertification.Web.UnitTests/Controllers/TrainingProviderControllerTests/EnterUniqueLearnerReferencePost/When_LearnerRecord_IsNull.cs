using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_LearnerRecord_IsNull : TestSetup
    {
        private readonly long uln = 123456789;
        private readonly FindLearnerRecord mockResult = null; 

        public override void Given()
        {
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = uln.ToString() };
            TrainingProviderLoader.FindLearnerRecordAsync(providerUkprn, uln).Returns(mockResult);
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
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(providerUkprn, uln);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberCriteria), uln.ToString());
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberNotFound),
                    Arg.Is<ProvidersUlnNotFoundViewModel>(x => x.Uln == uln.ToString()), CacheExpiryTime.XSmall);
        }
    }
}