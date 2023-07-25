using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism.SpecialismCodeConverter
{
    public class When_DualSpecialism : TestSetup
    {
        public override void Given()
        {
            Source = PlumbingAndHeatingEngineeringRegistration;
        }

        [Fact]
        public void Then_Return_SpecialismCode()
        {
            Result.Should().Be(PlumbingAndHeatingEngineering.LarId);
        }
    }
}