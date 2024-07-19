using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.DownloadResults;
using System;
using System.IO;
using System.Text;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResults
{
    public class When_Action_IsCalled : TestSetup
    {
        protected override DateTime CurrentDate => DateTime.UtcNow.AddDays(-1);

        public override void Given()
        {
            DownloadOverallResultsLoader.DownloadOverallResultSlipsAsync(ProviderUkprn)
                .Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for download overall result slips")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadOverallResultSlipsAsync(ProviderUkprn);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as DownloadOverallResultsViewModel;
            model.Should().NotBeNull();
            model.IsOverallResultsAvailable.Should().BeTrue();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(2);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Download_Learner_Results);
        }
    }
}
