using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminChangeEnglishResultsViewModel _cacheModel;
        private const string ErrorKey = "AdminReviewChangesLevelTwoEnglish";

        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            _cacheModel = new AdminChangeEnglishResultsViewModel
            {
                RegistrationPathwayId = 1,
                EnglishStatusTo = SubjectStatus.Achieved
            };

            Controller.ModelState.AddModelError(ErrorKey, ReviewChangesEnglishStatus.Validation_Contact_Name_Blank_Text);
            CacheService.GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey).Returns(_cacheModel);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var model = ActualResult.ShouldBeViewResult<AdminReviewChangesEnglishSubjectViewModel>();

            model.Should().NotBeNull();
        }
    }
}