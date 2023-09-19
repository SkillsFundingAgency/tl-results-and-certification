using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationSpecialismToSpecialismRommExtractData
{
    public class When_Doesnt_Contain_Core_Grade : TestSetup
    {
        public override void Given()
        {
            SetSourceResults(Array.Empty<TqSpecialismResult>());
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentSpecialismGrade.Should().BeEmpty();
            Destination.RommOpenedTimeStamp.Should().NotHaveValue();
            Destination.RommGrade.Should().BeEmpty();
            Destination.AppealOpenedTimeStamp.Should().NotHaveValue();
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}