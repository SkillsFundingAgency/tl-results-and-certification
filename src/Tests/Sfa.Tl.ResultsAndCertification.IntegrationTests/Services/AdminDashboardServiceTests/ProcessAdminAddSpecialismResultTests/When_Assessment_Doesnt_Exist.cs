using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResultTests
{
    public class When_Assessment_Doesnt_Exist : ProcessAdminAddSpecialismResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 1;

        private AddSpecialismResultRequest _request;
        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, SpecialismAssessmentId);
            CreateAdminDasboardService();
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddSpecialismResultAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            _result.Should().BeFalse();
        }
    }
}