﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommReviewChangesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminOpenSpecialismRommReviewChangesViewModel _viewModel;
        private const string ErrorKey = "AdminOpenSpecialismRommReviewChanges";

        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            Controller.ModelState.AddModelError(ErrorKey, AdminOpenSpecialismRommReviewChanges.Validation_Message);
        }

        public async override Task When()
        {
            _result = await Controller.AdminOpenSpecialismRommReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = _result.ShouldBeViewResult<AdminOpenSpecialismRommReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminOpenSpecialismRommReviewChanges.Validation_Message);
        }
    }
}