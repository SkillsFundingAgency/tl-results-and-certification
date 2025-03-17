using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadResultsRommsByAwardingOrganisationGet
{
    public class When_Romm_Missing_Returns_Expected : AdminDownloadResultsRommsByAwardingOrganisationBaseTest
    {
        private readonly DataExportResponse _coreResult = CreateDataExportResponse(ComponentType.Core);
        private readonly DataExportResponse _specialismResult = CreateDataExportResponse(ComponentType.Specialism);
        private readonly DataExportResponse _romm = CreateDataExportResponse(ComponentType.NotSpecified, false);

        public override void Given()
        {
            Loader.GetAwardingOrganisationDisplayName(Ukprn).Returns(DisplayName);
            ResultLoader.GenerateResultsExportAsync(Ukprn, UserEmail).Returns(new List<DataExportResponse> { _coreResult, _specialismResult });
            PostResultsLoader.GenerateRommsDataExportAsync(Ukprn, UserEmail).Returns(new List<DataExportResponse> { _romm });
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled() => CallExpectedMethods();

        [Fact]
        public void Then_Returns_Expected()
        {
            var viewResult = Result.ShouldBeViewResult<AdminDownloadResultsRommsByAwardingOrganisationViewModel>();

            viewResult.AwardingOrganisationUkprn.Should().Be(Ukprn);
            viewResult.AwardingOrganisationDisplayName.Should().Be(DisplayName);

            AssertDownloadLink(viewResult.CoreResultsDownloadLinkViewModel, _coreResult);
            AssertDownloadLink(viewResult.SpecialismResultsDownloadLinkViewModel, _specialismResult);
            viewResult.RommsDownloadLinkViewModel.Should().BeNull();

            viewResult.BackLink.RouteName.Should().Be(RouteConstants.AdminSelectAwardingOrganisation);
        }
    }
}