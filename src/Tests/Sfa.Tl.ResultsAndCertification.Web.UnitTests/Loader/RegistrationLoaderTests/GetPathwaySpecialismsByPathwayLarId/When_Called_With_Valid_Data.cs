using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarId
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = new PathwaySpecialisms
            {
                Id = 1,
                PathwayCode = "12345",
                PathwayName = "Test 1",
                Specialisms = new List<PathwaySpecialismCombination>
                {
                    new PathwaySpecialismCombination
                    {
                        SpecialismDetails = new List<SpecialismDetails>
                        {
                            new SpecialismDetails { Id = 1, Code = "11111111", Name = "Design" },
                            new SpecialismDetails { Id = 2, Code = "22222222", Name = "Engineering" }
                        }
                    },
                    new PathwaySpecialismCombination
                    {
                        SpecialismDetails = new List<SpecialismDetails>
                        {
                            new SpecialismDetails { Id = 3, Code = "33333333", Name = "Arts" },
                        }
                    }
                }
            };

            InternalApiClient.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, PathwayLarId).Returns(ApiClientResponse);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            // Test to ensure one couplet and one single are being read in ascending Order 
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ApiClientResponse.Id);
            ActualResult.PathwayName.Should().Be(ApiClientResponse.PathwayName);
            ActualResult.PathwayCode.Should().Be(ApiClientResponse.PathwayCode);

            ActualResult.Specialisms.Should().NotBeNullOrEmpty();
            ActualResult.Specialisms.Count.Should().Be(2);

            ActualResult.Specialisms[0].Code.Should().Be("33333333");
            ActualResult.Specialisms[0].DisplayName.Should().Be("Arts (33333333)");

            ActualResult.Specialisms[1].Code.Should().Be("11111111|22222222");
            ActualResult.Specialisms[1].DisplayName.Should().Be("Design (11111111) and Engineering (22222222)");
        }

        [Fact]
        public void Then_SpecialismLookup_Is_Populated()
        {
            ActualResult.SpecialismsLookup.Should().NotBeNullOrEmpty();
            ActualResult.SpecialismsLookup.Count.Should().Be(ApiClientResponse.Specialisms.SelectMany(x => x.SpecialismDetails).Count());

            var expectedLookup = ApiClientResponse.Specialisms.SelectMany(x => x.SpecialismDetails.Select(s => new KeyValuePair<string, string>(s.Code, s.Name)));
            ActualResult.SpecialismsLookup.Should().BeEquivalentTo(expectedLookup);
        }
    }
}
