using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadResultsRommsByAwardingOrganisationGet
{
    public class When_Results_ComponentType_NotSpecified_Redirect : AdminDownloadResultsRommsByAwardingOrganisationBaseTest
    {
        public override void Given()
        {
            Loader.GetAwardingOrganisationDisplayName(Ukprn).Returns(DisplayName);
            ResultLoader.GenerateResultsExportAsync(Ukprn, UserEmail).Returns(new List<DataExportResponse>
            {
                CreateDataExportResponse(ComponentType.NotSpecified)
            });

            PostResultsLoader.GenerateRommsDataExportAsync(Ukprn, UserEmail).Returns(new List<DataExportResponse>
            {
                CreateDataExportResponse(ComponentType.NotSpecified)
            });
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled() => CallExpectedMethods();

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToProblemWithService();
        }
    }
}