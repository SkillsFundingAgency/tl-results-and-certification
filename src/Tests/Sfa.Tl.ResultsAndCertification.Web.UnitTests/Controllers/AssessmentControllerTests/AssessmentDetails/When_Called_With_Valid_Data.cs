using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
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

            // Uln            
            model.SummaryUln.Title.Should().NotBeNull(AssessmentDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockresult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(AssessmentDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockresult.Firstname} {_mockresult.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(AssessmentDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockresult.DateofBirth.ToDobFormat());

            // Provider
            model.SummaryProvider.Title.Should().Be(AssessmentDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockresult.ProviderName}<br/>({_mockresult.ProviderUkprn})");

            // TlevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(AssessmentDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockresult.TlevelTitle);

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.AssessmentDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Assessment_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchAssessments);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Assessments);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Learners_Assessment_entries);
        }
    }
}
