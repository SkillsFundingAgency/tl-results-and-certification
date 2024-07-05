using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsDataFor
{
    public class When_Returns_No_DataFound_DataExportResponse : TestSetup
    {
        public override void Given()
        {
            DataExportResponse dataExportResponse = new()
            {
                IsDataFound = false
            };

            ProviderRegistrationsLoader.GetRegistrationsDataExportAsync(ProviderUkprn, StartYear, Email).Returns(dataExportResponse);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.RegistrationsNoRecordsFound);
        }
    }
}