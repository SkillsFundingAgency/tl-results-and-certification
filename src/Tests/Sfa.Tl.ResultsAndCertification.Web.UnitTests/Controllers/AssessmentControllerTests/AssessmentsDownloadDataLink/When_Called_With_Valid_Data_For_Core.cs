using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using System.IO;
using System.Text;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentsDownloadDataLink
{
    public class When_Called_With_Valid_Data_For_Core : TestSetup
    {
        public override void Given()
        {
            Id = Guid.NewGuid().ToString();
            ComponentType = Common.Enum.ComponentType.Core;
            AssessmentLoader.GetAssessmentsDataFileAsync(Ukprn, Id.ToGuid()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for core assessment entries")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentsDataFileAsync(Ukprn, Id.ToGuid());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(AssessmentContent.AssessmentsDownloadData.Core_Assessments_Download_FileName);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
