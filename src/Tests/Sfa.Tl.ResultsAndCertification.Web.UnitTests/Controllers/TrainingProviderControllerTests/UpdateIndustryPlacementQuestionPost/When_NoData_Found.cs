﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionPost
{
    public class When_NoData_Found : TestSetup
    {
        private UpdateLearnerRecordResponseViewModel _updateLearnerRecordResponse;
        public override void Given()
        {
            _updateLearnerRecordResponse = null;

            UpdateIndustryPlacementQuestionViewModel = new UpdateIndustryPlacementQuestionViewModel
            {
                ProfileId = 1,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };

            TrainingProviderLoader.ProcessIndustryPlacementQuestionUpdateAsync(ProviderUkprn, UpdateIndustryPlacementQuestionViewModel).Returns(_updateLearnerRecordResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).ProcessIndustryPlacementQuestionUpdateAsync(ProviderUkprn, UpdateIndustryPlacementQuestionViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
