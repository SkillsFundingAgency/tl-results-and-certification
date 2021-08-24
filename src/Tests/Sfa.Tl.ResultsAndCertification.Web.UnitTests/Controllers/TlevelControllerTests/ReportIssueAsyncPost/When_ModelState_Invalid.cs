using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation };
            Controller.ModelState.AddModelError("Query", Content.Tlevel.Query.Query_Required_Validation_Message);
            TlevelLoader.GetQueryTlevelViewModelAsync(AoUkprn, PathwayId).Returns(ExpectedResult);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(AoUkprn, PathwayId);
        }

        [Fact]
        public void Then_Returns_Expcted_ViewModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.TqAwardingOrganisationId.Should().Be(ExpectedResult.TqAwardingOrganisationId);
            model.RouteId.Should().Be(ExpectedResult.RouteId);
            model.PathwayId.Should().Be(ExpectedResult.PathwayId);
            model.PathwayStatusId.Should().Be(ExpectedResult.PathwayStatusId);

            model.IsBackToVerifyPage.Should().Be(ExpectedResult.IsBackToVerifyPage);
            model.Query.Should().Be(ExpectedResult.Query);

            model.TlevelTitle.Should().Be(ExpectedResult.TlevelTitle);
            model.PathwayDisplayName.Should().Be(ExpectedResult.PathwayDisplayName);
            model.Specialisms.Should().NotBeNull();
            model.Specialisms.Count().Should().Be(ExpectedResult.Specialisms.Count());
            model.Specialisms.First().Should().Be(ExpectedResult.Specialisms.First());

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(model.Query)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(model.Query)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Tlevel.Query.Query_Required_Validation_Message);
        }
    }
}
