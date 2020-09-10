using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismQuestionChangeAsync
{
    public class When_NoSpecialism_Selected : TestSetup
    {
        private ManageRegistration mockResponse = null;

        public override void Given()
        {
            mockResponse = new ManageRegistration
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

            ViewModel = new ChangeSpecialismQuestionViewModel { ProfileId = 1, HasLearnerDecidedSpecialism = null };
            InternalApiClient.GetRegistrationAsync(AoUkprn, Arg.Any<int>()).Returns(mockResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            InternalApiClient.Received(1).GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
            ActualResult.Should().BeNull();
        }
    }
}
