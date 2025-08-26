using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedSpecialismSeries.HighestAttainedSpecialismSeriesConverter
{
    public class When_Contains_Results : TestSetup
    {
        public override void Given()
        {
            var specialismWithBestResult = CreateSpecialism(Summer2022, DistinctionResult);
            var specialismWithLowerResult = CreateSpecialism(Summer2023, PassResult);

            Source = new List<TqRegistrationSpecialism>
            {
                specialismWithLowerResult,
                specialismWithBestResult
            };
        }

        [Fact]
        public void Then_Return_Series_Name_Of_Best_Result()
        {
            Result.Should().Be(Summer2022.Name);
        }
    }
}