using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProviderChanges
{
    public class When_ProviderChange_Success : TestSetup
    {
        private RegistrationDetails registrationApiClientResponse;
        private long _providerUkprn;

        public override void Given()
        {
            _providerUkprn = 12345678;

            registrationApiClientResponse = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = Uln,
                Firstname = "Test",
                Lastname = "Last",
                AoUkprn = AoUkprn,
                ProviderUkprn = 34567890,
                PathwayLarId = "10000112",
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

            ViewModel = new ChangeProviderViewModel { ProfileId = 1, SelectedProviderUkprn = _providerUkprn.ToString() };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(registrationApiClientResponse);
            InternalApiClient.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(mockProviderPathwayDetailsApiClientResponse);

            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == registrationApiClientResponse.Uln && 
                x.FirstName == registrationApiClientResponse.Firstname && 
                x.LastName == registrationApiClientResponse.Lastname && 
                x.ProviderUkprn == _providerUkprn &&
                x.AoUkprn == registrationApiClientResponse.AoUkprn))
                .Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
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
            ActualResult.IsSuccess.Should().BeTrue();
            ActualResult.ProfileId.Should().Be(ViewModel.ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
