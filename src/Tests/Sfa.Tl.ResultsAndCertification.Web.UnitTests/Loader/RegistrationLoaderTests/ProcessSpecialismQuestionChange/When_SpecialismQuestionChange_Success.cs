using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismQuestionChange
{
    public class When_SpecialismQuestionChange_Success : TestSetup
    {
        private RegistrationDetails registrationApiClientResponse;

        public override void Given()
        {
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

            ViewModel = new ChangeSpecialismQuestionViewModel { ProfileId = 1, HasLearnerDecidedSpecialism = false };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(registrationApiClientResponse);
            
            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == registrationApiClientResponse.Uln &&
                x.FirstName == registrationApiClientResponse.Firstname &&
                x.LastName == registrationApiClientResponse.Lastname &&
                x.ProviderUkprn == registrationApiClientResponse.ProviderUkprn &&
                x.CoreCode == registrationApiClientResponse.PathwayLarId &&
                x.SpecialismCodes.Count() == 0))
                .Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeTrue();
            ActualResult.ProfileId.Should().Be(ViewModel.ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
