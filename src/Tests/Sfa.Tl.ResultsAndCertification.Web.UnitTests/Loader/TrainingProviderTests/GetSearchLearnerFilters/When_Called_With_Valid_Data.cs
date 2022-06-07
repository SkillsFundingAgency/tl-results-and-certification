using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetSearchLearnerFilters
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private SearchLearnerFilters expectedApiResult;

        public override void Given()
        {
            expectedApiResult = new SearchLearnerFilters
            {
                AcademicYears = new List<FilterLookupData> 
                { 
                    new FilterLookupData { Id = 1, Name = "2020 to 2021", IsSelected = false },
                    new FilterLookupData { Id = 2, Name = "2021 to 2022", IsSelected = false }
                }
            };

            InternalApiClient.GetSearchLearnerFiltersAsync(ProviderUkprn).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            
            // Assert AcademicYears
            ActualResult.AcademicYears.Should().HaveCount(expectedApiResult.AcademicYears.Count);
            ActualResult.AcademicYears.Should().BeEquivalentTo(expectedApiResult.AcademicYears);
        }
    }
}
