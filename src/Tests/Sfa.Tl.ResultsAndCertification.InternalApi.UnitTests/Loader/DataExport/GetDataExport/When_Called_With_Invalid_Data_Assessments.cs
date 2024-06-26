﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.GetDataExport
{
    public class When_Called_With_Invalid_Data_Assessments : TestSetup
    {
        private IList<CoreAssessmentsExport> _coreAssessments;
        private IList<SpecialismAssessmentsExport> _specialismAssessments;

        public override void Given()
        {
            DataExportType = Common.Enum.DataExportType.Assessments;

            _coreAssessments = new List<CoreAssessmentsExport> { new CoreAssessmentsExport { Uln = 1111112548, StartYear= "2021/22", CoreCode = "45678941", CoreAssessmentEntry = "Summer 2021" } };
            _specialismAssessments = new List<SpecialismAssessmentsExport>();

            DataExportRepository.GetDataExportCoreAssessmentsAsync(AoUkprn).Returns(_coreAssessments);
            DataExportRepository.GetDataExportSpecialismAssessmentsAsync(AoUkprn).Returns(_specialismAssessments);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Response.Should().NotBeNull();
            Response.Count.Should().Be(2);

            Response.FirstOrDefault().IsDataFound.Should().BeTrue();
            Response.FirstOrDefault().ComponentType.Should().Be(Common.Enum.ComponentType.Core);

            Response.LastOrDefault().IsDataFound.Should().BeFalse();
            Response.LastOrDefault().ComponentType.Should().Be(Common.Enum.ComponentType.Specialism);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            DataExportRepository.Received(1).GetDataExportCoreAssessmentsAsync(AoUkprn);
            DataExportRepository.Received(1).GetDataExportSpecialismAssessmentsAsync(AoUkprn);
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
