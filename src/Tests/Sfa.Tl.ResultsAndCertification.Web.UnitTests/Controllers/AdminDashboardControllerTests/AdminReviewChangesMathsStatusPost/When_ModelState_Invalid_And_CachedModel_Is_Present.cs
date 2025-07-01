using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusPost
{

    public class When_ModelState_Invalid_And_CachedModel_Is_Present : TestSetup
    {
        private AdminChangeMathsResultsViewModel _cachedMathsModel;
        private const string ErrorKey = "ContactName";

        public override void Given()
        {
            // Simulating a validation failure for 'ContactName' as it
            // has the [Required] attribute
            ViewModel = CreateViewModel(SubjectStatus.Achieved);
            ViewModel.ContactName = "";

            _cachedMathsModel = new AdminChangeMathsResultsViewModel
            {
                RegistrationPathwayId = 1,
                MathsStatusTo = SubjectStatus.Achieved,
                LearnerName = "learner-name",
                Uln = 1234567890,
                Provider = "provider",
                TlevelName = "tlevel-name",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                MathsStatus = SubjectStatus.NotAchieved
            };

            CacheService.GetAsync<AdminChangeMathsResultsViewModel>(CacheKey).Returns(_cachedMathsModel);
            Controller.ModelState.AddModelError(ErrorKey, ReviewChangesMathsStatus.Validation_Contact_Name_Blank_Text);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeMathsResultsViewModel>(CacheKey);

            AdminDashboardLoader.DidNotReceive().ProcessChangeMathsStatusAsync(Arg.Any<AdminReviewChangesMathsSubjectViewModel>());

            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<NotificationBannerModel>(),
                Arg.Any<CacheExpiryTime>()
            );
        }

        [Fact]
        public void Then_Returns_View_With_Model_And_Errors()
        {
            var viewResult = ActualResult.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<AdminReviewChangesMathsSubjectViewModel>().Subject;

            model.Should().NotBeNull();

            model.AdminChangeResultsViewModel.Should().BeEquivalentTo(_cachedMathsModel);

            viewResult.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult.ViewData.ModelState.Should().ContainKey(ErrorKey);
            viewResult.ViewData.ModelState[ErrorKey].Errors.Should().ContainSingle(
                e => e.ErrorMessage == ReviewChangesMathsStatus.Validation_Contact_Name_Blank_Text);
        }
    }
}