using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class Then_Expected_Results_Returned : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {
            TlevelLoader.GetQueryTlevelViewModelAsync(ukprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_GetQueryTlevelViewModelAsync_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(ukprn, pathwayId);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returnes()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.PathwayId.Should().Be(expectedResult.PathwayId);
            model.PathwayName.Should().Be(expectedResult.PathwayName);
            model.PathwayStatusId.Should().Be(expectedResult.PathwayStatusId);
            model.Query.Should().Be(expectedResult.Query);
            model.TqAwardingOrganisationId.Should().Be(expectedResult.TqAwardingOrganisationId);

            model.Specialisms.Should().NotBeNull();
            model.Specialisms.Count().Should().Be(expectedResult.Specialisms.Count());
            model.Specialisms.First().Should().Be(expectedResult.Specialisms.First());
        }

        [Fact]
        public void Then_Returns_Expected_BackLinkModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.IsBackToVerifyPage.Should().BeFalse();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.TlevelDetails);
            model.BackLink.RouteAttributes.Count().Should().Be(1);
            model.BackLink.RouteAttributes["id"].Should().Be(model.PathwayId.ToString());
        }
    }
}
