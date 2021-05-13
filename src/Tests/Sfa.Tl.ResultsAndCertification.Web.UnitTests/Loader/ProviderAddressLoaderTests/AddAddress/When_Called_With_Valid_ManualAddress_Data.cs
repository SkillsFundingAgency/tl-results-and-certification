using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.AddAddress
{
    public class When_Called_With_Valid_ManualAddress_Data : TestSetup
    {
        private bool _expectedApiResult { get; set; }

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            _expectedApiResult = true;

            AddAddressViewModel = new AddAddressViewModel
            {
                AddAddressManual = new AddAddressManualViewModel { Department = "Test", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy" }
            };

            InternalApiClient.AddAddressAsync(Arg.Is<AddAddressRequest>(
                    x => x.Ukprn == ProviderUkprn &&
                    x.DepartmentName == AddAddressViewModel.AddAddressManual.Department &&
                    x.AddressLine1 == AddAddressViewModel.AddAddressManual.AddressLine1 &&
                    x.AddressLine2 == AddAddressViewModel.AddAddressManual.AddressLine2 &&
                    x.Town == AddAddressViewModel.AddAddressManual.Town &&
                    x.Postcode == AddAddressViewModel.AddAddressManual.Postcode &&
                    x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(_expectedApiResult);

            Loader = new ProviderAddressLoader(InternalApiClient, OrdnanceSurveyApiClient, Mapper, Logger);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().Be(_expectedApiResult);
        }
    }
}
