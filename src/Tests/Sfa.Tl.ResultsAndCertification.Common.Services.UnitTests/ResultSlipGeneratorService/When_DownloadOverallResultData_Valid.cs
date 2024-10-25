using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.ResultSlipGeneratorService
{
    public class When_DownloadOverallResultData_Valid : TestSetup
    {
        public override void Given()
        {
            Response = new MemoryStream(Encoding.ASCII.GetBytes("Test File for download overall result slips")).ToArray();
            Service.GetByteData(Arg.Any<IEnumerable<DownloadOverallResultSlipsData>>())
                .Returns(Response);
        }

        [Fact]
        public void Then_Returns_Expected_Exception()
        {
            Response.Should().NotBeNull();
        }
    }
}
