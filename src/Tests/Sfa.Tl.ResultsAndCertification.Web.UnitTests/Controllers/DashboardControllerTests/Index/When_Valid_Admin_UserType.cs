using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public class When_Valid_Admin_UserType : When_Valid_UserType_BaseTest
    {
        public override void Given()
        {
            Given(LoginUserType.Admin);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Then_Expected_Result(LoginUserType.Admin);
        }
    }
}