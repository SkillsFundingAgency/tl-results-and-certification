using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedSpecialismSeries.HighestAttainedSpecialismSeriesConverter
{
    public class When_Specialism_Assessment_Has_No_Result : TestSetup
    {
        public override void Given()
        {
            var specialismWithNoResult = CreateSpecialism(Summer2022);
            Source = new List<TqRegistrationSpecialism> { specialismWithNoResult };
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}