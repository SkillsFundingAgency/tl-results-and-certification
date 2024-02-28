using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.ProcessAddSpecialismResultReviewChanges
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private readonly AdminAddSpecialismResultReviewChangesViewModel _model = new()
        {
            RegistrationPathwayId = 1,
            SpecialismAssessmentId = 1,
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
            ApiClient.ProcessAdminAddSpecialismResultAsync(Arg.Any<AddSpecialismResultRequest>()).Returns(true);
        }

        public async override Task When()
        {
            _result = await Loader.ProcessAddSpecialismResultReviewChangesAsync(_model);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).ProcessAdminAddSpecialismResultAsync(Arg.Any<AddSpecialismResultRequest>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}