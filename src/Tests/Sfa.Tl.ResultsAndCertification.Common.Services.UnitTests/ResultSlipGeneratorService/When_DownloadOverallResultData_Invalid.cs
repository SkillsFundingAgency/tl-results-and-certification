using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.ResultSlipGeneratorService
{
    public class When_DownloadOverallResultData_Invalid : TestSetup
    {
        List<DownloadOverallResultSlipsData> _data;
        public override void Given()
        {
            Response = Service.GetByteData(_data);
        }

        [Fact]
        public void Then_Returns_Expected_Exception()
        {
            Response.Should().Throws<Exception>();
        }
    }
}
