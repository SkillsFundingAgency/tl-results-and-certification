﻿using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using FluentAssertions;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeWithdrawnStatusHaveYouToldAwardingOrganisationPost
{
    public  class When_Yes_Selected : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ChangeWithdrawnStatusHaveYouToldAwardingOrganisationViewModel
            {
                HaveYouToldAwardingOrganisation = true,
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 1123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader.UpdateLearnerWithdrawnStatusAsync(ProviderUkprn,ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).UpdateLearnerWithdrawnStatusAsync(ProviderUkprn,ViewModel);

            CacheService.Received(1).SetAsync(InformationCacheKey, Arg.Is<InformationBannerModel>(x =>
                x.Heading.Equals(Content.ViewComponents.InformationBanner.Heading) &&
                x.Message.Equals(string.Format(LearnerDetailsContent.Withdrawn_Message_Told_AO_Yes_Template, ViewModel.LearnerName, ViewModel.AwardingOrganisationName))),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var result = Result as RedirectToRouteResult;

            result.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);

            result.RouteValues.Should().HaveCount(1);
            result.RouteValues.Should().ContainKey("profileId");
            result.RouteValues["profileId"].Should().Be(ViewModel.ProfileId);
        }
    }
}
