using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_IsCoreEntryEligible_IsFalse : TestSetup
    {
        private AssessmentDetailsViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = null,
                PathwayAssessmentId = 5,
                IsResultExist = false,
                SpecialismDisplayName = "Specialism1 (2345678)",
                SpecialismAssessmentSeries = AssessmentDetailsContent.Available_After_Autumn2021,
                PathwayStatus = RegistrationPathwayStatus.Active,
                IsCoreEntryEligible = false
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
            model.ProviderDisplayName.Should().Be($"{mockresult.ProviderName}<br/>({mockresult.ProviderUkprn})");
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SpecialismDisplayName.Should().Be(mockresult.SpecialismDisplayName);
            model.SpecialismAssessmentSeries.Should().Be(mockresult.SpecialismAssessmentSeries);
            model.PathwayStatus.Should().Be(mockresult.PathwayStatus);
            model.IsResultExist.Should().BeFalse();

            // Summary CoreAssessment Entry            
            model.SummaryCoreAssessmentEntry.Should().NotBeNull();
            model.SummaryCoreAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummaryCoreAssessmentEntry.Value.Should().Be(AssessmentDetailsContent.Available_After_Current_Assessment_Series);
            model.SummaryCoreAssessmentEntry.ActionText.Should().BeNull();
            model.SummaryCoreAssessmentEntry.RenderHiddenActionText.Should().Be(true);
            model.SummaryCoreAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Core_Assessment_Entry_Hidden_Text);

            // Summary SpecialismAssessment Entry
            model.SummarySpecialismAssessmentEntry.Should().NotBeNull();
            model.SummarySpecialismAssessmentEntry.Title.Should().Be(AssessmentDetailsContent.Title_Assessment_Entry_Text);
            model.SummarySpecialismAssessmentEntry.Value.Should().Be(AssessmentDetailsContent.Available_After_Autumn2021);
            model.SummarySpecialismAssessmentEntry.ActionText.Should().BeNull();
            model.SummarySpecialismAssessmentEntry.RenderHiddenActionText.Should().Be(true);
            model.SummarySpecialismAssessmentEntry.HiddenActionText.Should().Be(AssessmentDetailsContent.Specialism_Assessment_Entry_Hidden_Text);

            // Breadcrum
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
