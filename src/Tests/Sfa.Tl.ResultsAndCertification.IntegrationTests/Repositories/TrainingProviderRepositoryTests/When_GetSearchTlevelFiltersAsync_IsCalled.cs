using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public class When_GetSearchTlevelFiltersAsync_IsCalled : TrainingProviderRepositoryBaseTest
    {
        private IList<FilterLookupData> _actualResult;

        public override void Given()
        {
            SeedTestData();
            SeedAdditionalPathways();

            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TraningProviderRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _actualResult = await TrainingProviderRepository.GetSearchTlevelFiltersAsync();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            _actualResult.Should().HaveCount(3);

            var expectedResult = new List<FilterLookupData>
            {
               new FilterLookupData { Id = 1, Name = "Design, Surveying and Planning", IsSelected = false },
               new FilterLookupData { Id = 3, Name = "Health", IsSelected = false },
               new FilterLookupData { Id = 2, Name = "Onsite Construction", IsSelected = false },
            };

            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private void SeedAdditionalPathways()
        {
            var tlPathwayList = new List<TlPathway>{
                new TlPathway
                {
                    TlevelTitle = "T Level in Onsite Construction",
                    Name = "Onsite Construction",
                    LarId = "60369176",
                    StartYear = 2020,
                    TlRouteId = Route.Id,
                    TlRoute = Route,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },

                new TlPathway
                {
                    TlevelTitle = "T Level in Health",
                    Name = "Health",
                    LarId = "6037066X",
                    StartYear = 2020,
                    TlRouteId = Route.Id,
                    TlRoute = Route,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };

            DbContext.TlPathway.AddRange(tlPathwayList);
            DbContext.SaveChanges();
        }
    }
}

