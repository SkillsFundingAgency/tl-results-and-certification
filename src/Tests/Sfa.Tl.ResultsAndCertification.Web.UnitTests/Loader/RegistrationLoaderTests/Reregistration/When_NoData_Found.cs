using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.Reregistration
{
    public class When_NoData_Found : TestSetup
    {
        readonly RegistrationDetails mockRegDetails = null;
        private string _coreCode;
        public override void Given()
        {
            _coreCode = "10000111";
            ViewModel = new ReregisterViewModel
            {
                ReregisterProvider = new ReregisterProviderViewModel { ProfileId = ProfileId },
                ReregisterCore = new ReregisterCoreViewModel { ProfileId = ProfileId, SelectedCoreCode = _coreCode }
            };
            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(mockRegDetails);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
