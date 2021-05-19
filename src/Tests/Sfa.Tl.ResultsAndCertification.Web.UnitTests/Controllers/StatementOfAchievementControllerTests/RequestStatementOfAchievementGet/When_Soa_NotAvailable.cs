using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestStatementOfAchievementGet
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
    }
}