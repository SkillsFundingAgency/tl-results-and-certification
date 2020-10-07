using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.Reregistration
{
    public class When_Selected_Core_NotSupported : TestSetup
    {
        private RegistrationDetails mockApiClientResponse;
        private string _coreCode = "20000113";
        public override void Given()
        {
            mockApiClientResponse = new RegistrationDetails
            {
                ProfileId = 1,
                ProviderUkprn = 3425678,
                PathwayLarId = _coreCode,
            };

            ViewModel = new ReregisterViewModel
            {
                ReregisterProvider = new ReregisterProviderViewModel { ProfileId = ProfileId },
                ReregisterCore = new ReregisterCoreViewModel { ProfileId = ProfileId, SelectedCoreCode = _coreCode }
            };
            
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(mockApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSelectedCoreSameAsWithdrawn.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeFalse();
        }
    }
}
