using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SubmitAssessmentsGeneratingDownload
{
    public class When_Called_With_Valid_Data : TestSetup
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
                    ComponentType = ComponentType.Core,
                    IsDataFound = true
                },
                new DataExportResponse
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 50,
                    ComponentType = ComponentType.Specialism,
                    IsDataFound = true
                }
            };

            AssessmentLoader.GenerateAssessmentsExportAsync(AoUkprn, Arg.Any<string>()).Returns(_dataExportResponse);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            var coreExportResponse = _dataExportResponse.FirstOrDefault(d => d.ComponentType == ComponentType.Core);
            var specialismExportResponse = _dataExportResponse.FirstOrDefault(d => d.ComponentType == ComponentType.Specialism);

            var coreDownloadLinkViewModel = new DownloadLinkViewModel
            {
                BlobUniqueReference = coreExportResponse.BlobUniqueReference,
                FileSize = coreExportResponse.FileSize,
                FileType = FileType.Csv.ToString().ToUpperInvariant()
            };

            var specialismDownloadLinkViewModel = new DownloadLinkViewModel
            {
                BlobUniqueReference = specialismExportResponse.BlobUniqueReference,
                FileSize = specialismExportResponse.FileSize,
                FileType = FileType.Csv.ToString().ToUpperInvariant()
            };

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AssessmentsDownloadViewModel>(x =>
                            x.CoreAssesmentsDownloadLinkViewModel.BlobUniqueReference == coreDownloadLinkViewModel.BlobUniqueReference &&
                            x.CoreAssesmentsDownloadLinkViewModel.FileSize == coreDownloadLinkViewModel.FileSize &&
                            x.CoreAssesmentsDownloadLinkViewModel.FileType == coreDownloadLinkViewModel.FileType &&
                            x.SpecialismAsssmentsDownloadLinkViewModel.BlobUniqueReference == specialismDownloadLinkViewModel.BlobUniqueReference &&
                            x.SpecialismAsssmentsDownloadLinkViewModel.FileSize == specialismDownloadLinkViewModel.FileSize &&
                            x.SpecialismAsssmentsDownloadLinkViewModel.FileType == specialismDownloadLinkViewModel.FileType),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentsDownloadData()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AssessmentsDownloadData);
        }        
    }
}