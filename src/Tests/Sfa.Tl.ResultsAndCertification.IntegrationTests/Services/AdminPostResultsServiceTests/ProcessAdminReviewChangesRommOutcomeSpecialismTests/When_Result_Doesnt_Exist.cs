using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminReviewChangesRommOutcommSpecialismTests
{
    public class When_Result_Doesnt_Exist : ProcessAdminReviewChangesRommOutcomeSpecialismBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismResultId = 1;

        private ReviewChangesRommOutcomeSpecialismRequest _request;
        private IAdminPostResultsService _service;

        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, SpecialismResultId);
            _service = CreateAdminPostResultsService();
        }

        public override async Task When()
        {
            _result = await _service.ProcessAdminReviewChangesRommOutcomeSpecialismAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            _result.Should().BeFalse();
        }
    }
}