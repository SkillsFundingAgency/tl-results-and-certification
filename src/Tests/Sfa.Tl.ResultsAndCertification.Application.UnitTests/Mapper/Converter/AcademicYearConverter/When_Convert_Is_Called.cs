using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.AcademicYearConverter
{
    public class When_Convert_Is_Called : TestSetup
    {
        public override void Given()
        {
            Source = 2020;
        }

        [Fact]
        public void Then_Return_Expected()
        {
            Result.Should().Be("2020 to 2021");
        }
    }
}