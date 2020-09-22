using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ReJoinRegistration
{
    public class When_ReJoin_Failed : TestSetup
    {
        ReJoinRegistrationRequest reJoinRequest = null;
        public override void Given()
        {
            ApiClientResponse = false;
            ViewModel = new ReJoinRegistrationViewModel { ProfileId = 1, Uln = Uln };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
            reJoinRequest = new ReJoinRegistrationRequest { AoUkprn = AoUkprn, ProfileId = ProfileId, PerformedBy = $"{Givenname} {Surname}" };
            InternalApiClient.ReJoinRegistrationAsync(Arg.Any<ReJoinRegistrationRequest>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).ReJoinRegistrationAsync(Arg.Any<ReJoinRegistrationRequest>());
        }

        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<ReJoinRegistrationRequest>(ViewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

            result.Should().NotBeNull();
            result.AoUkprn.Should().Be(AoUkprn);
            result.ProfileId.Should().Be(reJoinRequest.ProfileId);
            result.PerformedBy.Should().Be(reJoinRequest.PerformedBy);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().BeFalse();
            ActualResult.ProfileId.Should().Be(ViewModel.ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
