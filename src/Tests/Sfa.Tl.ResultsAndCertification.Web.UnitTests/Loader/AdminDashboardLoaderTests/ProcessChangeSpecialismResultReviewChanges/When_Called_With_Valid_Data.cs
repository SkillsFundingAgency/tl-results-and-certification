using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.ProcessChangeSpecialismResultReviewChanges
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private readonly AdminChangeSpecialismResultReviewChangesViewModel _model = new()
        {
            RegistrationPathwayId = 1,
            SpecialismAssessmentId = 1,
            SelectedGradeId = 1,
            ContactName = "test-contact-name",
            Day = "01",
            Month = "01",
            Year = "2024",
            ChangeReason = "test-change-reason",
            ZendeskTicketId = "12345",
            Grade = "B",
            SpecialismResultId = 1,
            SelectedGradeValue = "A*"
        };

        private bool _result;

        public override void Given()
        {
            ApiClient.ProcessAdminChangeSpecialismResultAsync(Arg.Any<ChangeSpecialismResultRequest>()).Returns(true);
        }

        public async override Task When()
        {
            _result = await Loader.ProcessChangeSpecialismResultReviewChangesAsync(_model);
        }

        
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}