using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddress
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Address _expectedApiResult;

        public override void Given()
        {
            ProviderUkprn = 9874561231;

            _expectedApiResult = new Address
            {
                DepartmentName = "Dept",
                OrganisationName = "Org",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                Town = "Town",
                Postcode = "x11 1yy"
            };
            InternalApiClient.GetAddressAsync(ProviderUkprn).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetAddressAsync(ProviderUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.DepartmentName.Should().Be(_expectedApiResult.DepartmentName);
            ActualResult.OrganisationName.Should().Be(_expectedApiResult.OrganisationName);
            ActualResult.AddressLine1.Should().Be(_expectedApiResult.AddressLine1);
            ActualResult.AddressLine2.Should().Be(_expectedApiResult.AddressLine2);
            ActualResult.Town.Should().Be(_expectedApiResult.Town);
            ActualResult.Postcode.Should().Be(_expectedApiResult.Postcode);
        }
    }
}
