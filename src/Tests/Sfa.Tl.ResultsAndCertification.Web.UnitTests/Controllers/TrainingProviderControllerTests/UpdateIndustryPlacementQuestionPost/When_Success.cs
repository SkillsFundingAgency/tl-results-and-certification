using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionPost
{
    public class When_Success : TestSetup
    {
        private UpdateLearnerRecordResponseViewModel _updateLearnerRecordResponse;

        public override void Given()
        {
            _updateLearnerRecordResponse = new UpdateLearnerRecordResponseViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test User",
                IsModified = true,
                IsSuccess = true
            };

            UpdateIndustryPlacementQuestionViewModel = new UpdateIndustryPlacementQuestionViewModel
            {
                ProfileId = 1,
                RegistrationPathwayId = 1,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };

            TrainingProviderLoader.ProcessIndustryPlacementQuestionUpdateAsync(ProviderUkprn, UpdateIndustryPlacementQuestionViewModel).Returns(_updateLearnerRecordResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).ProcessIndustryPlacementQuestionUpdateAsync(ProviderUkprn, UpdateIndustryPlacementQuestionViewModel);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.IndustryPlacementUpdatedConfirmation),
                Arg.Is<UpdateLearnerRecordResponseViewModel>
                (x => x.ProfileId == _updateLearnerRecordResponse.ProfileId &&
                      x.Name == _updateLearnerRecordResponse.Name &&
                      x.Uln == _updateLearnerRecordResponse.Uln),
                 CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_IndustryPlacementUpdatedConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IndustryPlacementUpdatedConfirmation);
        }
    }
}
