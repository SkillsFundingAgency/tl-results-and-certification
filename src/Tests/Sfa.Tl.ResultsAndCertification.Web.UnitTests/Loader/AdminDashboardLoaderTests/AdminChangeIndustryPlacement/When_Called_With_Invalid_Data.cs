using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminChangeIndustryPlacement
{
    public class When_Called_With_Invalid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private AdminIpCompletionViewModel _result;

        public override void Given()
        {
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(null as AdminLearnerRecord);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
        }
    }
}