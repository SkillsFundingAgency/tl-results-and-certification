using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.DownloadResults;
using System;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadResultsControllerTests.DownloadTlevelResults
{
    public class When_Action_IsCalled : TestSetup
    {
        private bool _expectedIsOverallResultsAvailable;

        public override void Given()
        {
            _expectedIsOverallResultsAvailable = ResultsAndCertificationConfiguration.OverallResultsAvailableDate >= DateTime.Today;
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as DownloadTlevelResultsViewModel;
            model.Should().NotBeNull();
            model.IsOverallResultsAvailable.Should().Be(_expectedIsOverallResultsAvailable);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(1);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
        }
    }
}
