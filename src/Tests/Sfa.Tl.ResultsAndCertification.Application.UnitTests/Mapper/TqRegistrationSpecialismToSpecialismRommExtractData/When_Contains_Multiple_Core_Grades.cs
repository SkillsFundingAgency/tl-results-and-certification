using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationSpecialismToSpecialismRommExtractData
{
    public class When_Contains_Multiple_Core_Grades : TestSetup
    {

        public override void Given()
        {
            TqSpecialismResult[] results = new[] 
            {
                CreateTqSpecialismResult(1, "C", false),
                CreateTqSpecialismResult(2, "B", true),
            };

            SetSourceResults(results);
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentSpecialismGrade.Should().Be("C");
            Destination.RommOpenedTimeStamp.Should().NotHaveValue();
            Destination.RommGrade.Should().BeEmpty();
            Destination.AppealOpenedTimeStamp.Should().NotHaveValue();
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}