using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override Task When() => Task.CompletedTask;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123456789")]
        [InlineData("12345678901")]
        [InlineData("0123456789")]
        [InlineData("abcdefghij")]
        public async Task Then_Returns_Expected_Results(string uln)
        {
            Model = Entry();
            Model.UlnTo = uln;
            var errors = new List<ValidationResult>();
            // MVC performs this validation before invoking the action.
            Validator.TryValidateObject(Model, new ValidationContext(Model), errors, true).Should().BeFalse();
            foreach (var error in errors)
                foreach (var property in error.MemberNames)
                    Controller.ModelState.AddModelError(property, error.ErrorMessage);

            await base.When();
            Result.Should().BeOfType<ViewResult>();
            Controller.ModelState.IsValid.Should().BeFalse();
            Controller.ModelState["UlnTo"].Errors[0].ErrorMessage.Should().Be("Enter a ULN with 10 digits");
            await AdminDashboardLoader.DidNotReceive().IsLearnerUlnAvailableAsync(Arg.Any<long>());
            await CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminChangeUlnViewModel>(), Arg.Any<CacheExpiryTime>());
        }
    }
}
