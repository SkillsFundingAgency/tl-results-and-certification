using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionGet
{
    public class When_Called_With_Invalid_ProfileId : TestSetup
    {
        private readonly UpdateIndustryPlacementQuestionViewModel mockresult = null;
        public override void Given()
        {
            ProfileId = 0;
            PathwayId = 1;
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateIndustryPlacementQuestionViewModel>(ProviderUkprn, ProfileId, PathwayId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<UpdateIndustryPlacementQuestionViewModel>(ProviderUkprn, ProfileId, PathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}