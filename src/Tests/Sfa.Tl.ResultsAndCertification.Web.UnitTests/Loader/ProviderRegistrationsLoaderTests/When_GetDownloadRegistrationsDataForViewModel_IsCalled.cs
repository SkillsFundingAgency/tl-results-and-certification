using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System;
using System.Threading.Tasks;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderRegistrationsLoaderTests
{
    public class When_GetDownloadRegistrationsDataForViewModel_IsCalled : ProviderRegistrationsLoaderBaseTest
    {
        private const int StartYear = 2020;
        private const int FileSize = 100;
        private readonly Guid BlobUniqueReference = new("b7a8d6e1-3c0e-4f1b-9c5a-2e1e1b4c8e6d");

        private DownloadRegistrationsDataForViewModel _actualResult;

        public override void Given()
        {
        }

        public override Task When()
        {
            DataExportResponse dataExportResponse = new()
            {
                IsDataFound = true,
                BlobUniqueReference = BlobUniqueReference,
                FileSize = FileSize,
                ComponentType = ComponentType.NotSpecified
            };

            _actualResult = Loader.GetDownloadRegistrationsDataForViewModel(dataExportResponse, StartYear);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.PageTitle.Should().Be("Download registrations data for 2020 to 2021");
            _actualResult.PageHeader.Should().Be("Download registrations data for 2020 to 2021");
            _actualResult.DownloadLinkText.Should().Be("Download T Levels registrations data 2020 to 2021");

            _actualResult.DownloadLink.BlobUniqueReference.Should().Be(BlobUniqueReference);
            _actualResult.DownloadLink.FileSize.Should().Be(FileSize);
            _actualResult.DownloadLink.FileType.Should().Be(FileType.Csv.ToString().ToUpperInvariant());

            _actualResult.Breadcrumb.BreadcrumbItems.Should().HaveCount(3);
            _actualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            _actualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            _actualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Download_Registrations_Data);
            _actualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.DownloadRegistrationsData);
            _actualResult.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be("Download registrations data for 2020 to 2021");
            _actualResult.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
        }
    }
}