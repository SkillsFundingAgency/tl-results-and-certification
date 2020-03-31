using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.GetProviderLookupData
{
    public class Then_On_Valid_ProviderName_Returns_Expected_JsonResults : When_FindProviderAsync_Post_Action_Is_Called
    {
        private List<ProviderLookupData> expectedResults;
        
        public override void Given() 
        {
            expectedResults = new List<ProviderLookupData>
            {
                new ProviderLookupData { Id = 1, DisplayName = "Provider 1" },
                new ProviderLookupData { Id = 2, DisplayName = "Provider 2" },
            };

            ProviderLoader.GetProviderLookupDataAsync(ProviderName, false)
                .Returns(expectedResults);
        }

        [Fact]
        public void Then_GetProviderLookupDataAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).GetProviderLookupDataAsync(ProviderName, false);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Result.Should().BeOfType(typeof(JsonResult));
            var actualResults = Result.Result.Value as IEnumerable<ProviderLookupData>;

            actualResults.Count().Should().Be(2);
            actualResults.First().Id.Should().Be(expectedResults.First().Id);
            actualResults.First().DisplayName.Should().Be(expectedResults.First().DisplayName);
        }
    }
}
