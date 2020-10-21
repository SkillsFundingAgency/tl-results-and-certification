using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.WithdrawRegistration
{
    public class When_Withdraw_Success : TestSetup
    {
        WithdrawRegistrationRequest withdrawRequest = null;
        public override void Given()
        {
            ApiClientResponse = true;
            ViewModel = new WithdrawRegistrationViewModel { ProfileId = 1, Uln = Uln };
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
            withdrawRequest = new WithdrawRegistrationRequest { AoUkprn = AoUkprn, ProfileId = ProfileId, PerformedBy = $"{Givenname} {Surname}" };
            InternalApiClient.WithdrawRegistrationAsync(Arg.Any<WithdrawRegistrationRequest>()).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).WithdrawRegistrationAsync(Arg.Any<WithdrawRegistrationRequest>());
        }

        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<WithdrawRegistrationRequest>(ViewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

            result.Should().NotBeNull();
            result.AoUkprn.Should().Be(AoUkprn);
            result.ProfileId.Should().Be(withdrawRequest.ProfileId);
            result.PerformedBy.Should().Be(withdrawRequest.PerformedBy);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().BeTrue();
            ActualResult.ProfileId.Should().Be(ViewModel.ProfileId);
            ActualResult.Uln.Should().Be(Uln);
        }
    }
}
