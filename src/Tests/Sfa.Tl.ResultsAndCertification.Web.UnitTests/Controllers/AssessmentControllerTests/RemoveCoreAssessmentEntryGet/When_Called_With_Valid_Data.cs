using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using RemoveCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.RemoveCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveCoreAssessmentEntryGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentEntryDetailsViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AssessmentEntryDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                AssessmentId = 3,
                AssessmentSeriesName = "Summer 2022"
            };

            AssessmentLoader.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ProfileId, ComponentType.Core).Returns(_mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentEntryDetailsViewModel));

            var model = viewResult.Model as AssessmentEntryDetailsViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.AssessmentId.Should().Be(_mockresult.AssessmentId);
            model.AssessmentSeriesName.Should().Be(_mockresult.AssessmentSeriesName);

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(RemoveCoreAssessmentEntryContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(RemoveCoreAssessmentEntryContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(RemoveCoreAssessmentEntryContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(RemoveCoreAssessmentEntryContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(RemoveCoreAssessmentEntryContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockresult.TlevelTitle);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_mockresult.ProfileId.ToString());
        }
    }
}
