using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.AddAddress
{
    public class When_Called_With_Valid_SelectAddress_Data : TestSetup
    {
        private bool _expectedApiResult { get; set; }

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            _expectedApiResult = true;

            AddAddressViewModel = new AddAddressViewModel
            {
                AddAddressPostcode = new AddAddressPostcodeViewModel { Postcode = "xx1 1yy" },
                AddAddressSelect = new AddAddressSelectViewModel { SelectedAddressUprn = 123456789, SelectedAddress = new AddressViewModel { DepartmentName ="Dept name", OrganisationName = "Org name", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy" } }
            };

            InternalApiClient.AddAddressAsync(Arg.Is<AddAddressRequest>(
                    x => x.Ukprn == ProviderUkprn &&
                    x.DepartmentName == AddAddressViewModel.AddAddressSelect.DepartmentName &&
                    x.OrganisationName == AddAddressViewModel.AddAddressSelect.SelectedAddress.OrganisationName &&
                    x.AddressLine1 == AddAddressViewModel.AddAddressSelect.SelectedAddress.AddressLine1 &&
                    x.AddressLine2 == AddAddressViewModel.AddAddressSelect.SelectedAddress.AddressLine2 &&
                    x.Town == AddAddressViewModel.AddAddressSelect.SelectedAddress.Town &&
                    x.Postcode == AddAddressViewModel.AddAddressSelect.SelectedAddress.Postcode &&
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
