using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public class When_Valid_AwardingOrganisation_UserType : When_Valid_UserType_BaseTest
    {
        public override void Given()
        {
            Given(LoginUserType.AwardingOrganisation);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Then_Expected_Result(LoginUserType.AwardingOrganisation);
        }
    }
}
