using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionGet
{
    public class When_Called_With_IsLearnerRecordAdded_False : TestSetup
    {
        private UpdateIndustryPlacementQuestionViewModel mockresult = null;
        public override void Given()
        {
            ProfileId = 10;
            PathwayId = 15;
            mockresult = new UpdateIndustryPlacementQuestionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = PathwayId,
                IsLearnerRecordAdded = false
            };
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
