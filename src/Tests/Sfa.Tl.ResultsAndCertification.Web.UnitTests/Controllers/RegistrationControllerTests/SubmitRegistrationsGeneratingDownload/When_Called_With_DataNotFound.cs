using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SubmitRegistrationsGeneratingDownload
{
    public class When_Called_With_DataNotFound : TestSetup
    {
        public override void Given()
        {
            DataExportResponse response = new()
            {
                IsDataFound = false
            };

            RegistrationLoader.GenerateRegistrationsExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { response });
            RegistrationLoader.GeneratePendingWithdrawalsExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { response });
        }

        [Fact]
        public void Then_Redirected_To_RegistrationsNoRecordsFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationsNoRecordsFound);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<RegistrationsDownloadViewModel>(),
                Arg.Any<CacheExpiryTime>());
        }
    }
}
