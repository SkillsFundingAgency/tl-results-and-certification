using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminLearnerRecordWithOccupationalSpecialism
{
    public class When_Called_With_Invalid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int RegistrationSpecialismId = 1;
        private AdminOccupationalSpecialismViewModel _result;

        public override void Given()
        {
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(null as AdminLearnerRecord);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordWithOccupationalSpecialism(RegistrationPathwayId, RegistrationSpecialismId);
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