﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Assessments
{
    public class When_Assessments_NotFound : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
        }

        [Fact]
        public void Then_Returns_Erorr_HeaderNotFound()
        {
            var actualResult = Response;
            actualResult.IsDirty.Should().BeTrue();
            actualResult.Rows.Count().Should().Be(0);
            actualResult.ErrorMessage.Should().Be(ValidationMessages.NoRecordsFound);
            actualResult.ErrorCode.Should().Be(CsvFileErrorCode.NoRecordsFound);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine(AssessmentEntryHeader);
            return csvData;
        }
    }
}
