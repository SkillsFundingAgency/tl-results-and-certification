using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class Then_Invalid_Header_Return_IsDirty_Validation_Error : When_ReadAndParseFileAsync_Is_Called
    {
        public override void Given()
        {
            InputStream = new MemoryStream(Encoding.ASCII.GetBytes("Test File"));
        }

        [Fact]
        public void Then_Returns_Header_Not_Found_Error()
        {
            Response.Result.IsDirty.Should().BeTrue();
            Response.Result.Rows.Count().Should().Be(0);
            Response.Result.ErrorMessage.Should().Be(ValidationMessages.FileHeaderNotFound);
        }
    }
}
