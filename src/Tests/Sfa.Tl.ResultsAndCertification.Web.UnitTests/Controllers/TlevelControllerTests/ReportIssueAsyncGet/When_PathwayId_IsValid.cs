using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Linq;
using Xunit;

using SummaryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.TlevelSummary;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class When_PathwayId_IsValid : TestSetup
    {
        public override void Given()
        {
            TlevelLoader.GetQueryTlevelViewModelAsync(AoUkprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(AoUkprn, pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.TqAwardingOrganisationId.Should().Be(expectedResult.TqAwardingOrganisationId);
            model.RouteId.Should().Be(expectedResult.RouteId);
            model.PathwayId.Should().Be(expectedResult.PathwayId);
            model.PathwayStatusId.Should().Be(expectedResult.PathwayStatusId);

            model.IsBackToVerifyPage.Should().Be(expectedResult.IsBackToVerifyPage);
            model.Query.Should().Be(expectedResult.Query);

            model.TlevelTitle.Should().Be(expectedResult.TlevelTitle);
            model.PathwayDisplayName.Should().Be(expectedResult.PathwayDisplayName);
            model.Specialisms.Should().NotBeNull();
            model.Specialisms.Count().Should().Be(expectedResult.Specialisms.Count());
            model.Specialisms.First().Should().Be(expectedResult.Specialisms.First());

            // Summary SummaryTlevelTitle            
            model.SummaryTlevelTitle.Should().NotBeNull();
            model.SummaryTlevelTitle.Title.Should().Be(SummaryContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(expectedResult.TlevelTitle);

            // Summary SummaryTlevelTitle            
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(SummaryContent.Title_Core_Code_Text);
            model.SummaryCore.Value.Should().Be(expectedResult.PathwayDisplayName);

            // Summary SummaryTlevelTitle            
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(SummaryContent.Title_Occupational_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeEquivalentTo(expectedResult.Specialisms);
        }

        [Fact]
        public void Then_Returns_Expected_BackLinkModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.IsBackToVerifyPage.Should().BeFalse();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.TlevelDetails);
            model.BackLink.RouteAttributes.Count().Should().Be(1);
            model.BackLink.RouteAttributes["id"].Should().Be(model.PathwayId.ToString());
        }
    }
}
