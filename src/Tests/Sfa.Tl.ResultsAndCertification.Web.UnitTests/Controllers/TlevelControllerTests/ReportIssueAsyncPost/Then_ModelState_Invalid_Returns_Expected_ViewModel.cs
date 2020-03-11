using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class Then_ModelState_Invalid_Returns_Expected_ViewModel : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation };
            Controller.ModelState.AddModelError("Query", "Please enter a query.");
            TlevelLoader.GetQueryTlevelViewModelAsync(Ukprn, PathwayId).Returns(ExpectedResult);
        }

        [Fact]
        public void Then_GetQueryTlevelViewModelAsync_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(Ukprn, PathwayId);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returns()
        {
            var viewResult = Result.Result as ViewResult;
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
