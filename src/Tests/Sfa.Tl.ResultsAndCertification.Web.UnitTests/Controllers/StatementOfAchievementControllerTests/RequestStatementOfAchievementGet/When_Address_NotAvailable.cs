using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestStatementOfAchievementGet
{
    public class When_Address_NotAvailable : TestSetup
    {
        private readonly Address _address = null;
        public override void Given()
        {
            ResultsAndCertificationConfiguration.SoaAvailableDate = DateTime.UtcNow.AddDays(-30);
            ProviderAddressLoader.GetAddressAsync<Address>(ProviderUkprn).Returns(_address);
        }

        [Fact]
        public void Then_Redirected_To_PostalAddressMissing()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PostalAddressMissing);
        }
    }
}