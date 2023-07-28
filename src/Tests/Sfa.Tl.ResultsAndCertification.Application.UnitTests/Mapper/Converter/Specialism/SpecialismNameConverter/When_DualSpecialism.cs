using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism.SpecialismNameConverter
{
    public class When_DualSpecialism : TestSetup
    {
        public override void Given()
        {
            Source = Source = PlumbingAndHeatingEngineeringRegistration;
        }

        [Fact]
        public void Then_Return_SpecialismCode()
        {
            Result.Should().Be($"\"{PlumbingAndHeatingEngineering.Name}\"");
        }
    }
}