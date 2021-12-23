﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SubmitAssessmentsGeneratingDownload
{
    public class When_Called_With_NoData : TestSetup
    {
        private IList<DataExportResponse> _dataExportResponse;

        public override void Given()
        {
            _dataExportResponse = new List<DataExportResponse>
            {
                new DataExportResponse
                {
                    ComponentType = ComponentType.Core,
                    IsDataFound = false
                },
                new DataExportResponse
                {
                    ComponentType = ComponentType.Specialism,
                    IsDataFound = false
                }
            };

            AssessmentLoader.GenerateAssessmentsExportAsync(AoUkprn, Arg.Any<string>()).Returns(_dataExportResponse);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AssessmentsDownloadViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_AssessmentsNoRecordsFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.AssessmentsNoRecordsFound);
        }        
    }
}
