using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminDashboardRepositoryTests
{
    public class When_GetAwardingOrganisationFiltersAsync_IsCalled : BaseTest<TlAwardingOrganisation>
    {
        private readonly AdminDashboardRepository _repository;

        private IList<FilterLookupData> _actualResult;

        public When_GetAwardingOrganisationFiltersAsync_IsCalled()
        {
            _repository = new AdminDashboardRepository(DbContext);
        }

        public override void Given()
        {
            SeedAwardingOrganisations();
        }

        public override async Task When()
        {
            _actualResult = await _repository.GetAwardingOrganisationFiltersAsync();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(IList<FilterLookupData> expectedResult)
        {
            await When();

            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(expectedResult.Count);
            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new List<FilterLookupData>
                        {
                            new FilterLookupData { Id = 1, Name = "Ncfe", IsSelected = false },
                            new FilterLookupData { Id = 2, Name = "Pearson", IsSelected = false }
                        }
                    }
                };
            }
        }

        private void SeedAwardingOrganisations()
        {
            var builder = new TlAwardingOrganisationBuilder();

            IList<TlAwardingOrganisation> _awardingOrganisations = builder.BuildList();
            DbContext.AddRange(_awardingOrganisations);

            DbContext.SaveChanges();
        }
    }
}