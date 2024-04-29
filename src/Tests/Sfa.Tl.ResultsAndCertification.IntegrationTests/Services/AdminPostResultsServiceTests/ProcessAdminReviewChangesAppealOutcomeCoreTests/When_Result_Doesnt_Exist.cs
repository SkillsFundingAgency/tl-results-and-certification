using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminReviewChangesAppealOutcommCoreTests
{
    public class When_Result_Doesnt_Exist : ProcessAdminReviewChangesAppealOutcomeSpecialismBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayResultId = 1;

        private ReviewChangesAppealOutcomeCoreRequest _request;
        private IAdminPostResultsService _service;

        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, PathwayResultId);
            _service = CreateAdminPostResultsService();
        }

        public override async Task When()
        {
            _result = await _service.ProcessAdminReviewChangesAppealOutcomeCoreAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            _result.Should().BeFalse();
        }
    }
}