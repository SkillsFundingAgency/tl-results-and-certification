using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmitPost
{
    public class When_Soa_NotAvailable : TestSetup
    {
        public override void Given()
        {
            ResultsAndCertificationConfiguration.SoaAvailableDate = DateTime.UtcNow.AddDays(30);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Page()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.StatementsOfAchievementNotAvailable);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            StatementOfAchievementLoader.DidNotReceive().GetSoaLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>());
            StatementOfAchievementLoader.DidNotReceive().CreateSoaPrintingRequestAsync(Arg.Any<long>(), Arg.Any<SoaLearnerRecordDetailsViewModel>());
        }
    }
}
