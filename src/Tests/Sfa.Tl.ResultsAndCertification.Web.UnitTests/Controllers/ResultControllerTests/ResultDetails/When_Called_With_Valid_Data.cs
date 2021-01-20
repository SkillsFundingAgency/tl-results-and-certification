using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ResultDetailsViewModel mockresult = null;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            mockresult = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                SpecialismDisplayName = "Specialism1 (2345678)",
                PathwayResult = "A",
                PathwayResultId = 123, 
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ResultId, mockresult.PathwayResultId.ToString() } };
            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SpecialismDisplayName.Should().Be(mockresult.SpecialismDisplayName);
            model.PathwayStatus.Should().Be(mockresult.PathwayStatus);

            // Summary CoreResult
            model.SummaryCoreResult.Should().NotBeNull();
            model.SummaryCoreResult.Title.Should().Be(ResultDetailsContent.Title_Result_Text);
            model.SummaryCoreResult.Value.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SummaryCoreResult.ActionText.Should().Be(ResultDetailsContent.Change_Result_Action_Link_Text);
            model.SummaryCoreResult.RenderHiddenActionText.Should().Be(true);
            model.SummaryCoreResult.HiddenActionText.Should().Be(ResultDetailsContent.Core_Result_Hidden_Text);
            model.SummaryCoreResult.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Breadcrum
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Result_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchResults);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Results);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Learners_Results);
        }
    }
}
