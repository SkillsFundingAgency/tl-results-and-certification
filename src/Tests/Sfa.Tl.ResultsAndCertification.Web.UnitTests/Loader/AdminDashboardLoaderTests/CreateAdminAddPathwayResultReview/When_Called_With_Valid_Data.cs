using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.CreateAdminAddPathwayResultReview
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private readonly AdminAddPathwayResultReviewChangesViewModel _model = new()
        {
            RegistrationPathwayId = 1,
            PathwayAssessmentId = 1,
            SelectedGradeId = 1,
            ContactName = "test-contact-name",
            Day = "01",
            Month = "01",
            Year = "2024",
            ChangeReason = "test-change-reason",
            ZendeskTicketId = "12345"
        };

        private bool _result;

        public override void Given()
        {
            ApiClient.ProcessAdminAddPathwayResultAsync(Arg.Any<AddPathwayResultRequest>()).Returns(true);
        }

        public async override Task When()
        {
            _result = await Loader.ProcessAddPathwayResultReviewChangesAsync(_model);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).ProcessAdminAddPathwayResultAsync(Arg.Any<AddPathwayResultRequest>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}