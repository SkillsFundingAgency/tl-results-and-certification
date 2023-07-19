using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism.SpecialismCodeConverter
{
    public class When_Specialisms_Empty : TestSetup
    {
        public override void Given()
        {
            Source = Enumerable.Empty<TqRegistrationSpecialism>();
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}