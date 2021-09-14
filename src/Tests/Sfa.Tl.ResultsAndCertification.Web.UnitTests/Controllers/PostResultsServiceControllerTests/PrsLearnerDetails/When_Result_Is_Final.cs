using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_Result_Is_Final : TestSetup
    {
        private PrsLearnerDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;

            _mockLearnerDetails = new PrsLearnerDetailsViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                Status = RegistrationPathwayStatus.Active,

                ProviderName = "Barsely College",
                ProviderUkprn = 9876543210,

                TlevelTitle = "Tlevel in Childcare",
                PathwayTitle = "Childcare (12121212)",

                PathwayAssessmentSeries = "Summer 2021",
                PathwayResultId = 99,
                PathwayGrade = "B",
                PathwayPrsStatus = null,
                AppealEndDate = DateTime.Today.AddDays(-7),
                PathwayGradeLastUpdatedOn = DateTime.Today.AddDays(-5).ToString(),
                PathwayGradeLastUpdatedBy = "Barsley User"
            };

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsLearnerDetailsViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_mockLearnerDetails.ProfileId);
            model.Uln.Should().Be(_mockLearnerDetails.Uln);
            model.Firstname.Should().Be(_mockLearnerDetails.Firstname);
            model.Lastname.Should().Be(_mockLearnerDetails.Lastname);
            model.DateofBirth.Should().Be(_mockLearnerDetails.DateofBirth);
            model.Status.Should().Be(_mockLearnerDetails.Status);

            model.ProviderName.Should().Be(_mockLearnerDetails.ProviderName);
            model.ProviderUkprn.Should().Be(_mockLearnerDetails.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockLearnerDetails.TlevelTitle);
            model.PathwayTitle.Should().Be(_mockLearnerDetails.PathwayTitle);

            model.PathwayAssessmentSeries.Should().Be(_mockLearnerDetails.PathwayAssessmentSeries);
            model.AppealEndDate.Should().Be(_mockLearnerDetails.AppealEndDate);
            model.PathwayResultId.Should().Be(_mockLearnerDetails.PathwayResultId);
            model.PathwayGrade.Should().Be(_mockLearnerDetails.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_mockLearnerDetails.PathwayPrsStatus);
            model.PathwayGradeLastUpdatedOn.Should().Be(_mockLearnerDetails.PathwayGradeLastUpdatedOn);
            model.PathwayGradeLastUpdatedBy.Should().Be(_mockLearnerDetails.PathwayGradeLastUpdatedBy);
            model.IsFinalOutcomeRegistered.Should().BeFalse();
            model.IsAppealAllowedAfterDeadline.Should().BeTrue();
            model.SuccessBanner.Should().BeNull();

            // Uln
            model.SummaryUln.Title.Should().Be(LearnerDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockLearnerDetails.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(LearnerDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockLearnerDetails.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockLearnerDetails.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(LearnerDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_mockLearnerDetails.ProviderDisplayName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockLearnerDetails.TlevelTitle);

            // Assessment Series
            model.SummaryAssessmentSeries.Title.Should().Be(LearnerDetailsContent.Title_Assessment_Series);
            model.SummaryAssessmentSeries.Value.Should().Be(_mockLearnerDetails.PathwayAssessmentSeries);
            model.SummaryAssessmentSeries.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryAssessmentSeries.RenderEmptyRowForValue2.Should().Be(IsValidPathwayPrsStatus);

            // Pathway Grade
            model.SummaryPathwayGrade.Title.Should().Be(LearnerDetailsContent.Title_Pathway_Grade);
            model.SummaryPathwayGrade.Value.Should().Be(_mockLearnerDetails.PathwayGrade);
            model.SummaryPathwayGrade.Value2.Should().Be(CommonHelper.GetPrsStatusDisplayText(_mockLearnerDetails.PathwayPrsStatus, _mockLearnerDetails.AppealEndDate));
            model.SummaryPathwayGrade.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryPathwayGrade.RenderEmptyRowForValue2.Should().Be(IsValidPathwayPrsStatus);
            model.SummaryPathwayGrade.Value2CustomCssClass.Should().Be(Constants.TagFloatRightClassName);
            model.SummaryPathwayGrade.RenderActionColumn.Should().BeFalse();
            model.SummaryPathwayGrade.RenderHiddenActionText.Should().BeTrue();
            model.SummaryPathwayGrade.ActionText.Should().BeNull();
            model.SummaryPathwayGrade.RouteName.Should().BeNull();
            model.SummaryPathwayGrade.RouteAttributes.Should().BeNull();
            model.SummaryPathwayGrade.HiddenActionText.Should().BeNull();

            // Pathway grade last updated on
            model.SummaryPathwayGradeLastUpdatedOn.Title.Should().Be(LearnerDetailsContent.Title_Pathway_Grade_LastUpdatedOn);
            model.SummaryPathwayGradeLastUpdatedOn.Value.Should().Be(_mockLearnerDetails.PathwayGradeLastUpdatedOn);
            model.SummaryPathwayGradeLastUpdatedOn.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryAssessmentSeries.RenderEmptyRowForValue2.Should().Be(IsValidPathwayPrsStatus);

            // Pathway grade last updated by
            model.SummaryPathwayGradeLastUpdatedBy.Title.Should().Be(LearnerDetailsContent.Title_Pathway_Grade_LastUpdatedBy);
            model.SummaryPathwayGradeLastUpdatedBy.Value.Should().Be(_mockLearnerDetails.PathwayGradeLastUpdatedBy);
            model.SummaryPathwayGradeLastUpdatedBy.NeedBorderBottomLine.Should().BeTrue();
            model.SummaryAssessmentSeries.RenderEmptyRowForValue2.Should().Be(IsValidPathwayPrsStatus);

            // Breadcrum 
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.StartReviewsAndAppeals);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.StartReviewsAndAppeals);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Prs_Learner_Component_Grade_Status);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNull();
        }

        private bool IsValidPathwayPrsStatus => _mockLearnerDetails.PathwayPrsStatus.HasValue && _mockLearnerDetails.PathwayPrsStatus != PrsStatus.NotSpecified;
    }
}
