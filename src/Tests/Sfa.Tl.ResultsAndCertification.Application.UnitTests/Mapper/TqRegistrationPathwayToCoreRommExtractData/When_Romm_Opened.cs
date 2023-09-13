using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToCoreRommExtractData
{
    public class When_Romm_Opened : TestSetup
    {
        private readonly DateTime _rommOpenedTimeStamp = new(2023, 1, 1);

        public override void Given()
        {
            TqPathwayResult[] results = new[] 
            { 
                CreateTqPathwayResult(1, "C", false),
                CreateTqPathwayResult(2, "B", false),
                CreateTqPathwayResult(3, "B", true, PrsStatus.UnderReview, _rommOpenedTimeStamp)
            };

            SetSourceResults(results);
        }

        [Fact]
        public void Then_Map_As_Expected()
        {
            AssertDirectPropertyMappings();

            Destination.CurrentCoreGrade.Should().Be("B");
            Destination.RommOpenedTimeStamp.Should().Be(_rommOpenedTimeStamp);
            Destination.RommGrade.Should().BeEmpty();
            Destination.AppealOpenedTimeStamp.Should().NotHaveValue();
            Destination.AppealGrade.Should().BeEmpty();
        }
    }
}