using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistration
{
    public class Then_Cache_Remove_Is_Called : When_AddRegistration_Action_Is_Called
    {
        public override void Given() {}

        [Fact]
        public void Then_Cache_Remove_Method_Is_Called()
        {
            CacheService.Received(1).RemoveAsync<RegistrationViewModel>(CacheKey);
        }
    }
}
