using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_No_Assessments_Added : TestSetup
    {
        private AssessmentDetailsViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = null,
                SpecialismAssessmentSeries = null,
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            AssessmentLoader.GetAssessmentDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentDetailsViewModel));

            var model = viewResult.Model as AssessmentDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SpecialismDisplayName.Should().Be(mockresult.SpecialismDisplayName);
            model.SpecialismAssessmentSeries.Should().Be(mockresult.SpecialismAssessmentSeries);
            model.PathwayStatus.Should().Be(mockresult.PathwayStatus);

            // Summary CoreAssessment Entry            
            model.SummaryCoreAssessmentEntry.Should().NotBeNull();
            model.SummaryCoreAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummaryCoreAssessmentEntry.Value.Should().Be(AssessmentDetailsContent.Not_Specified_Text);
            model.SummaryCoreAssessmentEntry.ActionText.Should().Be(AssessmentDetailsContent.Add_Entry_Action_Link_Text);
            model.SummaryCoreAssessmentEntry.RenderHiddenActionText.Should().Be(true);
            model.SummaryCoreAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Core_Assessment_Entry_Hidden_Text);
            
            // Summary SpecialismAssessment Entry
            model.SummarySpecialismAssessmentEntry.Should().NotBeNull();
            model.SummarySpecialismAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummarySpecialismAssessmentEntry.Value.Should().Be(AssessmentDetailsContent.Available_After_Autumn2021);
            model.SummarySpecialismAssessmentEntry.ActionText.Should().BeNull();
            model.SummarySpecialismAssessmentEntry.RenderHiddenActionText.Should().Be(true);
            model.SummarySpecialismAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Specialism_Assessment_Entry_Hidden_Text);
        }
    }
}
