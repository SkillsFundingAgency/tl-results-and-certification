using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminChangeEnglishStatusViewModel _cacheModel;
        private const string ErrorKey = "AdminReviewChangesLevelTwoEnglish";

        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            _cacheModel = new AdminChangeEnglishStatusViewModel
            {
                RegistrationPathwayId = 1,
                EnglishStatusTo = SubjectStatus.Achieved
            };

            Controller.ModelState.AddModelError(ErrorKey, ReviewChangesEnglishStatus.Validation_Contact_Name_Blank_Text);
            CacheService.GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey).Returns(_cacheModel);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var model = ActualResult.ShouldBeViewResult<AdminReviewChangesEnglishStatusViewModel>();

            model.Should().NotBeNull();
        }
    }
}