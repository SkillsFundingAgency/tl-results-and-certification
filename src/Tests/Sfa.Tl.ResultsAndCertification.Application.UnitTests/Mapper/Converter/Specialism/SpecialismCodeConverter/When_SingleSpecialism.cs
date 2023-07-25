using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism.SpecialismCodeConverter
{
    public class When_SingleSpecialism : TestSetup
    {
        public override void Given()
        {
            Source = CivilEngineeringRegistration;
        }

        [Fact]
        public void Then_Return_SpecialismCode()
        {
            Result.Should().Be(CivilEngineering.LarId);
        }
    }
}