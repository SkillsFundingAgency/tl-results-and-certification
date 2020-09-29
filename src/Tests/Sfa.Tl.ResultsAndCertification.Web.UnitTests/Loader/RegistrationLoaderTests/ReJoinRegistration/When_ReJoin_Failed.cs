using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.RejoinRegistration
{
    public class When_Rejoin_Failed : TestSetup
    {
        RejoinRegistrationRequest reJoinRequest = null;
        public override void Given()
        {
            ApiClientResponse = false;
            ViewModel = new RejoinRegistrationViewModel { ProfileId = 1, Uln = Uln };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
            reJoinRequest = new RejoinRegistrationRequest { AoUkprn = AoUkprn, ProfileId = ProfileId, PerformedBy = $"{Givenname} {Surname}" };
            InternalApiClient.RejoinRegistrationAsync(Arg.Any<RejoinRegistrationRequest>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).RejoinRegistrationAsync(Arg.Any<RejoinRegistrationRequest>());
        }

        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<RejoinRegistrationRequest>(ViewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

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
