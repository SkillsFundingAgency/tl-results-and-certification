using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismQuestionChange
{
    public class When_NoSpecialism_Selected : TestSetup
    {
        private RegistrationDetails mockResponse = null;

        public override void Given()
        {
            mockResponse = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = Uln,
                Firstname = "Test",
                Lastname = "Last",
                AoUkprn = AoUkprn,
                ProviderUkprn = 34567890,
                PathwayLarId = "10000112",
            };

            ViewModel = new ChangeSpecialismQuestionViewModel { ProfileId = 1, HasLearnerDecidedSpecialism = null };
            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active)
                .Returns(mockResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            ActualResult.Should().BeNull();
        }
    }
}
