using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AddCoreAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AddCoreAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AddAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022"
            };

            AssessmentLoader.GetAddAssessmentEntryViewModelAsync(AoUkprn, ProfileId, ComponentType.Core).Returns(_mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddAssessmentEntryViewModel));

            var model = viewResult.Model as AddAssessmentEntryViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.AssessmentSeriesId.Should().Be(_mockresult.AssessmentSeriesId);
            model.AssessmentSeriesName.Should().Be(_mockresult.AssessmentSeriesName);
            model.IsOpted.Should().BeNull();

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(AddCoreAssessmentEntryContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(AddCoreAssessmentEntryContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(AddCoreAssessmentEntryContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(AddCoreAssessmentEntryContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(AddCoreAssessmentEntryContent.Title_TLevel_Text);
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
