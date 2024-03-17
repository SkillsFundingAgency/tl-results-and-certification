using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.ProcessChangeSpecialismResultReviewChanges
{
    public class When_Called_With_Invalid_Data : AdminDashboardLoaderTestsBase
    {
        private bool _result;

        public override void Given()
        {
        }

        public async override Task When()
        {
            _result = await Loader.ProcessChangeSpecialismResultReviewChangesAsync(null);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.DidNotReceive().ProcessAdminChangeSpecialismResultAsync(Arg.Any<ChangeSpecialismResultRequest>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeFalse();
        }
    }
}