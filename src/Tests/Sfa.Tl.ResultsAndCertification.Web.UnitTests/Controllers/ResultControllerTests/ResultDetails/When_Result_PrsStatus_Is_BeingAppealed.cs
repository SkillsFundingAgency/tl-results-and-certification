using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Result_PrsStatus_Is_BeingAppealed : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.Now.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567891,
                TlevelTitle = "Tlevel title",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                AppealEndDate = DateTime.Today.AddDays(7),
                PathwayAssessmentId = 11,
                PathwayResult = "A",
                PathwayResultId = 123,
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(_mockResult.Uln);
            model.Firstname.Should().Be(_mockResult.Firstname);
            model.Lastname.Should().Be(_mockResult.Lastname);
            model.LearnerName.Should().Be($"{_mockResult.Firstname} {_mockResult.Lastname}");
            model.DateofBirth.Should().Be(_mockResult.DateofBirth);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.ProviderDisplayName.Should().Be($"{_mockResult.ProviderName}<br/>({_mockResult.ProviderUkprn})");
            model.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            model.PathwayDisplayName.Should().Be(_mockResult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_mockResult.PathwayAssessmentSeries);
            model.PathwayAssessmentId.Should().Be(_mockResult.PathwayAssessmentId);
            model.PathwayResult.Should().Be(_mockResult.PathwayResult);
            model.PathwayStatus.Should().Be(_mockResult.PathwayStatus);
            model.IsValid.Should().Be(_mockResult.IsValid);
            model.IsResultAddOrChangeAllowed.Should().Be(_mockResult.IsResultAddOrChangeAllowed);


            // Uln
            model.SummaryUln.Title.Should().Be(ResultDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockResult.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(ResultDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockResult.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(ResultDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockResult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(ResultDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_mockResult.ProviderDisplayName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(ResultDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockResult.TlevelTitle);

            // ExamPeriod
            model.SummaryAssessmentSeries.Title.Should().Be(ResultDetailsContent.Title_Assessment_Series);
            model.SummaryAssessmentSeries.Value.Should().Be(_mockResult.PathwayAssessmentSeries);

            // Summary CoreResult
            model.SummaryPathwayGrade.Should().NotBeNull();
            model.SummaryPathwayGrade.Title.Should().Be(ResultDetailsContent.Title_Pathway_Grade);
            model.SummaryPathwayGrade.Value.Should().Be(_mockResult.PathwayResult);
            model.SummaryPathwayGrade.Value2.Should().Be(CommonHelper.GetPrsStatusDisplayText(_mockResult.PathwayPrsStatus, _mockResult.AppealEndDate));
            model.SummaryPathwayGrade.Value2CustomCssClass.Should().Be(Constants.TagFloatRightClassName);
            model.SummaryPathwayGrade.RenderActionColumn.Should().Be(_mockResult.IsResultAddOrChangeAllowed);
            model.SummaryPathwayGrade.ActionText.Should().BeNull();
            model.SummaryPathwayGrade.RouteName.Should().BeNull();
            model.SummaryPathwayGrade.HiddenActionText.Should().BeNull();
            model.SummaryPathwayGrade.HiddenValueText.Should().BeNull();
            model.SummaryPathwayGrade.RouteAttributes.Should().BeNull();

            // Breadcrumbs
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Result_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchResults);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Results);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Learners_Results);
        }
    }
}
