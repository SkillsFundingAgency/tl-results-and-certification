using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismQuestionChangeAsync
{
    public class When_SpecialismQuestionChange_Success : TestSetup
    {
        private ManageRegistration registrationApiClientResponse;

        public override void Given()
        {
            registrationApiClientResponse = new ManageRegistration
            {
                ProfileId = 1,
                Uln = Uln,
                FirstName = "Test",
                LastName = "Last",
                AoUkprn = AoUkprn,
                ProviderUkprn = 34567890,
                CoreCode = "10000112",
                PerformedBy = "updatedUser"
            };

            ViewModel = new ChangeSpecialismQuestionViewModel { ProfileId = 1, HasLearnerDecidedSpecialism = false };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);

            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId).Returns(registrationApiClientResponse);
            InternalApiClient.UpdateRegistrationAsync(Arg.Any<ManageRegistration>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetRegistrations()
        {
            InternalApiClient.Received(1).GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
        }

        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map(ViewModel, registrationApiClientResponse);

            result.Should().NotBeNull();

            result.AoUkprn.Should().Be(AoUkprn);
            result.Uln.Should().Be(registrationApiClientResponse.Uln);
            result.FirstName.Should().Be(registrationApiClientResponse.FirstName);
            result.LastName.Should().Be(registrationApiClientResponse.LastName);
            result.ProviderUkprn.Should().Be(registrationApiClientResponse.ProviderUkprn);
            result.CoreCode.Should().Be(registrationApiClientResponse.CoreCode);
            result.SpecialismCodes.Should().HaveCount(0);
            result.PerformedBy.Should().Be(registrationApiClientResponse.PerformedBy);
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
