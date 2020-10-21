using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.Reregistration
{
    public class When_Failed : TestSetup
    {
        private RegistrationDetails mockApiClientResponse;
        private long _providerUkprn;
        private string _coreCode;

        public override void Given()
        {
            _coreCode = "10000111";
            _providerUkprn = 12345678;
            ApiClientResponse = false;

            mockApiClientResponse = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = Uln,
                ProviderUkprn = _providerUkprn,
                PathwayLarId = "70000111",
            };

            ViewModel = new ReregisterViewModel
            {
                ReregisterProvider = new ReregisterProviderViewModel { ProfileId = ProfileId },
                ReregisterCore = new ReregisterCoreViewModel { ProfileId = ProfileId, SelectedCoreCode = _coreCode }
            };

            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(mockApiClientResponse);
            InternalApiClient.ReregistrationAsync(Arg.Any<ReregistrationRequest>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
            InternalApiClient.Received(1).ReregistrationAsync(Arg.Any<ReregistrationRequest>());
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSelectedCoreSameAsWithdrawn.Should().BeFalse();
            ActualResult.IsSuccess.Should().BeFalse();
            ActualResult.ProfileId.Should().Be(ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
