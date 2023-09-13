using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToSpecialismRommExtractData
{
    public class When_Appeal_Opened : TestSetup
    {
        private readonly DateTime _rommOpenedTimeStamp = new(2023, 1, 1);
        private readonly DateTime _appealOpenedTimeStamp = new(2023, 8, 31);

        public override void Given()
        {
            TqSpecialismResult[] results = new[] 
            {
                CreateTqSpecialismResult(1, "C", false),
                CreateTqSpecialismResult(2, "B", false),
                CreateTqSpecialismResult(3, "A", false, PrsStatus.Reviewed, _rommOpenedTimeStamp),
                CreateTqSpecialismResult(4, "A", true, PrsStatus.BeingAppealed, _appealOpenedTimeStamp)
            };

            SetSourceResults(results);
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentSpecialismGrade.Should().Be("B");
            Destination.RommOpenedTimeStamp.Should().Be(_rommOpenedTimeStamp);
            Destination.RommGrade.Should().Be("A");
            Destination.AppealOpenedTimeStamp.Should().Be(_appealOpenedTimeStamp);
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}