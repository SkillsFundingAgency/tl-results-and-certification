using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateLearnerRecord
{
    public class When_Action_Called : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).RemoveAsync<SearchLearnerRecordViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SearchLearnerRecord);
        }
    }
}
