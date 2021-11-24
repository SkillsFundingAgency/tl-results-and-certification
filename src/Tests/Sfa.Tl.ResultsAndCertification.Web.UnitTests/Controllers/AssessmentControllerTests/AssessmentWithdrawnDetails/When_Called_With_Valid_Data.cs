using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentWithdrawnDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentWithdrawnDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentWithdrawnDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentUlnWithdrawnViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AssessmentUlnWithdrawnViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname ="First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                TlevelTitle = "TLevel in Test",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentUlnWithdrawnViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(_mockresult);         
        }        

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentUlnWithdrawnViewModel));

            var model = viewResult.Model as AssessmentUlnWithdrawnViewModel;
            model.Should().NotBeNull();

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(AssessmentWithdrawnDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(AssessmentWithdrawnDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(AssessmentWithdrawnDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(AssessmentWithdrawnDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(AssessmentWithdrawnDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockresult.TlevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchAssessments);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }
    }
}
