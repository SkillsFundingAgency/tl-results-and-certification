using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism.SpecialismNameConverter
{
    public class When_Specialisms_Null : TestSetup
    {
        public override void Given()
        {
            Source = null;
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}