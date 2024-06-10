using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SubmitRegistrationsGeneratingDownload
{
    public class When_Called_With_Valid : TestSetup
    {
        private readonly DataExportResponse _registrationsResponse = new()
        {
            BlobUniqueReference = new Guid("f47d7a4e-9b8c-4a6f-8e4d-2e3b1a5c9f0d"),
            FileSize = 100,
            ComponentType = ComponentType.NotSpecified,
            IsDataFound = true
        };

        private readonly DataExportResponse _pendingWithdrawalsResponse = new()
        {
            BlobUniqueReference = new Guid("c8a3e9b7-5f8d-4b6a-9e8c-1d7f0e2c4a9f"),
            FileSize = 200,
            ComponentType = ComponentType.NotSpecified,
            IsDataFound = true
        };

        public override void Given()
        {
            RegistrationLoader.GenerateRegistrationsExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { _registrationsResponse });
            RegistrationLoader.GeneratePendingWithdrawalsExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { _pendingWithdrawalsResponse });
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
            string fileType = FileType.Csv.ToString().ToUpperInvariant();

            CacheService.Received(1).SetAsync(
                CacheKey,
                Arg.Is<RegistrationsDownloadViewModel>(x =>
                    x.RegistrationsDownloadLinkViewModel.BlobUniqueReference == _registrationsResponse.BlobUniqueReference
                    && x.RegistrationsDownloadLinkViewModel.FileSize == _registrationsResponse.FileSize
                    && x.RegistrationsDownloadLinkViewModel.FileType == fileType
                    && x.PendingWithdrawalsDownloadLinkViewModel.BlobUniqueReference == _pendingWithdrawalsResponse.BlobUniqueReference
                    && x.PendingWithdrawalsDownloadLinkViewModel.FileSize == _pendingWithdrawalsResponse.FileSize
                    && x.PendingWithdrawalsDownloadLinkViewModel.FileType == fileType),
                CacheExpiryTime.XSmall);
        }
    }
}
