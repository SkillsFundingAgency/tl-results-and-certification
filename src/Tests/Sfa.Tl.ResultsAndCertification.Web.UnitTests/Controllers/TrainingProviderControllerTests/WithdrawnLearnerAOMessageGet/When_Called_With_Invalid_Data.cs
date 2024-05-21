using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.WithdrawnLearnerAOMessageGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ProfileId = 0;
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<WithdrawLearnerAOMessageViewModel>(ProviderUkprn, ProfileId).Returns(null as WithdrawLearnerAOMessageViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Caled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<WithdrawLearnerAOMessageViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
