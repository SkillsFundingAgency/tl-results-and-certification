using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProviderChanges
{
    public class When_Provider_Unchanged : TestSetup
    {
        private ManageRegistration mockApiClientResponse;
        private long _providerUkprn;
        public override void Given()
        {
            _providerUkprn = 12345678;
            ViewModel = new ChangeProviderViewModel { ProfileId = 1, SelectedProviderUkprn = _providerUkprn.ToString() };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            mockApiClientResponse = new ManageRegistration
            {
                ProfileId = 1,
                ProviderUkprn = _providerUkprn,
                PerformedBy = "updatedUser"
            };
            
            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId).Returns(mockApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsModified.Should().BeFalse();
            ActualResult.IsCoreNotSupported.Should().BeFalse();
            ActualResult.IsSuccess.Should().BeFalse();
        }
    }
}
