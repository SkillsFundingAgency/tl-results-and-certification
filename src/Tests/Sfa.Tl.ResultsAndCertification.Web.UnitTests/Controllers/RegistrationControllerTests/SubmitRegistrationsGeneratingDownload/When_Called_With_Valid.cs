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
    public class When_Called_With_Valid : TestSetup
    {
        private IList<DataExportResponse> _dataExportResponse;

        public override void Given()
        {
            _dataExportResponse = new List<DataExportResponse>
            {
                new DataExportResponse
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 100,
                    ComponentType = Common.Enum.ComponentType.Core,
                    IsDataFound = true
                }
            };

            RegistrationLoader.GenerateRegistrationsExportAsync(AoUkprn, Arg.Any<string>())
                .Returns(_dataExportResponse);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationsDownloadData()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationsDownloadData);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<RegistrationsDownloadViewModel>(x =>
                            x.BlobUniqueReference == _dataExportResponse.FirstOrDefault().BlobUniqueReference &&
                            x.FileSize == _dataExportResponse.FirstOrDefault().FileSize &&
                            x.FileType == FileType.Csv.ToString().ToUpperInvariant()),
                            CacheExpiryTime.XSmall);
        }
    }
}
