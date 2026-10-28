using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests
{
    public class ChangeUlnTests : AdminDashboardControllerTestBase
    {
        public override Task When() => Task.CompletedTask;

        private AdminChangeUlnViewModel Entry() => new()
        {
            RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991"
        };

        private ReviewChangeUlnViewModel Learner() => new()
        {
            RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991",
            FirstName = "Susan", LastName = "Altman", ContactName = "Contact",
            Day = "1", Month = "1", Year = "2026", ChangeReason = "Correction"
        };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123456789")]
        [InlineData("12345678901")]
        [InlineData("0123456789")]
        [InlineData("abcdefghij")]
        public async Task Invalid_uln_returns_entry_screen(string uln)
        {
            var model = Entry();
            model.UlnTo = uln;
            var errors = new List<ValidationResult>();
            // MVC performs this validation before invoking the action.
            Assert.False(Validator.TryValidateObject(model, new ValidationContext(model), errors, true));
            foreach (var error in errors)
                foreach (var property in error.MemberNames)
                    Controller.ModelState.AddModelError(property, error.ErrorMessage);

            Assert.IsType<ViewResult>(await Controller.ChangeUlnAsync(3, model));
            Assert.False(Controller.ModelState.IsValid);
            Assert.Equal("Enter a ULN with 10 digits", Controller.ModelState["UlnTo"].Errors[0].ErrorMessage);
            await AdminDashboardLoader.DidNotReceive().IsAdminUlnAvailableAsync(Arg.Any<long>());
            await CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminChangeUlnViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public async Task Duplicate_returns_expected_error()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(3).Returns(Entry());
            AdminDashboardLoader.IsAdminUlnAvailableAsync(9999999991).Returns(false);
            Assert.IsType<ViewResult>(await Controller.ChangeUlnAsync(3, Entry()));
            Assert.Equal("ULN is already registered", Controller.ModelState["UlnTo"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Valid_uln_caches_review_and_redirects()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(3).Returns(Entry());
            AdminDashboardLoader.IsAdminUlnAvailableAsync(9999999991).Returns(true);
            var posted = Entry();
            posted.Uln = 1111111111;
            var result = Assert.IsType<RedirectToRouteResult>(await Controller.ChangeUlnAsync(3, posted));
            Assert.Equal(RouteConstants.AdminReviewChangeUln, result.RouteName);
            await CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AdminChangeUlnViewModel>(m => m.UlnTo == "9999999991" && m.Uln == 9999999990), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public async Task Entry_get_uses_cached_entry_model()
        {
            var model = Entry();
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(model);
            var result = Assert.IsType<ViewResult>(await Controller.ChangeUlnAsync(3));
            Assert.Same(model, result.Model);
            await AdminDashboardLoader.DidNotReceive().GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(Arg.Any<int>());
        }

        [Fact]
        public async Task Review_get_combines_learner_details_with_cached_entry()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            var learner = Learner();
            learner.UlnTo = null;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeUlnViewModel>(3).Returns(learner);
            var result = Assert.IsType<ViewResult>(await Controller.ReviewChangeUlnAsync(3));
            var model = Assert.IsType<ReviewChangeUlnViewModel>(result.Model);
            Assert.Equal("9999999991", model.UlnTo);
            Assert.Equal(9999999990, model.Uln);
            await CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<ReviewChangeUlnViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public async Task Starting_new_change_clears_entry_model()
        {
            await Controller.ChangeUlnClearAsync(3);
            await CacheService.Received(1).RemoveAsync<AdminChangeUlnViewModel>(CacheKey);
        }

        [Fact]
        public async Task Missing_review_cache_returns_not_found()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeUlnViewModel>(3).Returns(Learner());
            var result = Assert.IsType<RedirectToRouteResult>(await Controller.ReviewChangeUlnAsync(3));
            Assert.Equal(RouteConstants.PageNotFound, result.RouteName);
        }

        [Fact]
        public async Task Invalid_review_returns_model_without_saving()
        {
            Controller.ModelState.AddModelError(nameof(ReviewChangeUlnViewModel.ChangeReason), "Enter the reason why this change is needed");
            var model = Learner();
            var result = Assert.IsType<ViewResult>(await Controller.ReviewChangeUlnAsync(3, model));
            Assert.Same(model, result.Model);
            await AdminDashboardLoader.DidNotReceive().ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>());
        }

        [Fact]
        public async Task Duplicate_on_save_returns_entry_screen_with_validation_error()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            AdminDashboardLoader.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>()).Returns(false);
            AdminDashboardLoader.IsAdminUlnAvailableAsync(9999999991).Returns(false);

            var result = Assert.IsType<ViewResult>(await Controller.ReviewChangeUlnAsync(3, Learner()));
            Assert.Equal("ChangeUln", result.ViewName);
            Assert.IsType<AdminChangeUlnViewModel>(result.Model);
            Assert.Equal("ULN is already registered", Controller.ModelState["UlnTo"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Failed_save_returns_service_problem()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            AdminDashboardLoader.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>()).Returns(false);
            AdminDashboardLoader.IsAdminUlnAvailableAsync(9999999991).Returns(true);

            var result = Assert.IsType<RedirectToRouteResult>(await Controller.ReviewChangeUlnAsync(3, Learner()));
            Assert.Equal(RouteConstants.ProblemWithService, result.RouteName);
        }

        [Fact]
        public async Task Submit_uses_cached_identity_and_ulns_instead_of_hidden_fields()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            AdminDashboardLoader.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>()).Returns(true);
            var posted = Learner();
            posted.RegistrationPathwayId = 100; posted.Uln = 1111111111; posted.UlnTo = "2222222222";
            var result = Assert.IsType<RedirectToRouteResult>(await Controller.ReviewChangeUlnAsync(3, posted));
            Assert.Equal(RouteConstants.AdminLearnerRecord, result.RouteName);
            await AdminDashboardLoader.Received(1).ProcessChangeUlnAsync(Arg.Is<ReviewChangeUlnViewModel>(r =>
                r.RegistrationPathwayId == 3 && r.Uln == 9999999990 && r.UlnTo == "9999999991"));
        }
    }
}
