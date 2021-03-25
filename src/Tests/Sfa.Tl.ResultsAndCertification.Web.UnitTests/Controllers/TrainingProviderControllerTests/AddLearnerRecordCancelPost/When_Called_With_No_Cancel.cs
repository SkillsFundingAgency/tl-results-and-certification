using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordCancelPost
{
    public class When_Called_With_No_Cancel : TestSetup
    {
        public override void Given()
        {
            LearnerRecordCancelViewModel = new LearnerRecordCancelViewModel { CancelLearnerRecord = false };
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.DidNotReceive().RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }
    }
}
