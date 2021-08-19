using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation };
            Controller.ModelState.AddModelError("Query", "Please enter a query.");
            TlevelLoader.GetQueryTlevelViewModelAsync(AoUkprn, PathwayId).Returns(ExpectedResult);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(AoUkprn, PathwayId);
        }

        [Fact]
        public void Then_Returns_Expcted_ViewModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.PathwayId.Should().Be(ExpectedResult.PathwayId);
            model.PathwayName.Should().Be(ExpectedResult.PathwayName);
            model.PathwayStatusId.Should().Be(ExpectedResult.PathwayStatusId);
            model.Query.Should().Be(ExpectedResult.Query);
            model.TqAwardingOrganisationId.Should().Be(ExpectedResult.TqAwardingOrganisationId);

            model.Specialisms.Should().NotBeNull();
            model.Specialisms.Count().Should().Be(ExpectedResult.Specialisms.Count());
            model.Specialisms.First().Should().Be(ExpectedResult.Specialisms.First());
        }
    }
}
