using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProviderChangesTests
{
    public class When_ProviderChange_Failed : TestSetup
    {
        private ManageRegistration mockApiClientResponse;
        private long _providerUkprn;

        public override void Given()
        {
            _providerUkprn = 12345678;
            ApiClientResponse = false;

            mockApiClientResponse = new ManageRegistration
            {
                ProfileId = 1,
                Uln = Uln,
                ProviderUkprn = 76543678,
                CoreCode = "10000111",
                PerformedBy = "updatedUser"
            };

            var mockProviderPathwayDetailsApiClientResponse = new List<PathwayDetails>
            {
                new PathwayDetails
                {
                    Id = 1,
                    Name = "Test",
                    Code = "10000111"
                },
                new PathwayDetails
                {
                    Id = 2,
                    Name = "Display",
                    Code = "10000112"
                }
            };

            ViewModel = new ChangeProviderViewModel { ProfileId = 1, SelectedProviderUkprn = _providerUkprn.ToString(), };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId).Returns(mockApiClientResponse);
            InternalApiClient.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(mockProviderPathwayDetailsApiClientResponse);
            InternalApiClient.UpdateRegistrationAsync(Arg.Any<ManageRegistration>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetProviderPathwayDetails()
        {
            InternalApiClient.Received(1).GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsCoreNotSupported.Should().BeFalse();
            ActualResult.IsSuccess.Should().BeFalse();
            ActualResult.ProfileId.Should().Be(ViewModel.ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
