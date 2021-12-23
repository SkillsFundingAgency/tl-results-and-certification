using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SubmitRegistrationsGeneratingDownload
{
    public class When_Called_With_DataNotFound : TestSetup
    {
        private IList<DataExportResponse> _dataExportResponse;

        public override void Given()
        {
            _dataExportResponse = new List<DataExportResponse>
            {
                new DataExportResponse
                {
                    IsDataFound = false
                }
            };

            RegistrationLoader.GenerateRegistrationsExportAsync(AoUkprn, Arg.Any<string>())
                .Returns(_dataExportResponse);
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
