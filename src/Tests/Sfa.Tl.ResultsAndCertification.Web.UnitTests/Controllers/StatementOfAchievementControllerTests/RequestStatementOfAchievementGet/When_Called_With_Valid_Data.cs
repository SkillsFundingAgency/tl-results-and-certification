using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestStatementOfAchievementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Address _address;
        public override void Given()
        {
            ResultsAndCertificationConfiguration.SoaAvailableDate = DateTime.UtcNow.AddDays(-10);

            _address = new Address
            {
                DepartmentName = "Dept",
                OrganisationName = "Org",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                Town = "Town",
                Postcode = "x11 1yy"
            };
            ProviderAddressLoader.GetAddressAsync<Address>(ProviderUkprn).Returns(_address);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ProviderAddressLoader.Received(1).GetAddressAsync<Address>(ProviderUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as RequestStatementOfAchievementViewModel;
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.Home);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
