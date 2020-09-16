using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeCoreQuestionGet
{
    public class When_Cache_Found : TestSetup
    {
        private ChangeProviderViewModel cacheResult;
        private ChangeCoreQuestionViewModel mockresult = null;

        public override void Given()
        {
            cacheResult = new ChangeProviderViewModel
            {
                SelectedProviderDisplayName = "Test (12345678)"
            };

            CacheService.GetAndRemoveAsync<ChangeProviderViewModel>(CacheKey).Returns(cacheResult);

            mockresult = new ChangeCoreQuestionViewModel
            {
                ProfileId = 1,
                CoreDisplayName = "Test core (987654321)"
            };
            RegistrationLoader.GetRegistrationChangeCoreQuestionDetailsAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expecected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationChangeCoreQuestionDetailsAsync(AoUkprn, ProfileId);
        }
    }
}
