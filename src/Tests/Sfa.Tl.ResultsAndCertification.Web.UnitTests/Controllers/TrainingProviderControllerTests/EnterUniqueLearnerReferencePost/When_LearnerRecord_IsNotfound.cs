﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_LearnerRecord_IsNotfound : TestSetup
    {
        private readonly long uln = 123456789;
        private readonly bool _evaluteSendConfirmation = true;
        private FindLearnerRecord mockResult;

        public override void Given()
        {
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = uln.ToString() };
            mockResult = new FindLearnerRecord { IsLearnerRegistered = false };
            TrainingProviderLoader.FindLearnerRecordAsync(ProviderUkprn, uln, _evaluteSendConfirmation).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(ProviderUkprn, uln, _evaluteSendConfirmation);
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<AddLearnerRecordViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_EnterUniqueLearnerNumberNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumberNotFound);
        }
    }
}