using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.CommonRepositoryTests
{
    public class When_GetLoggedInUserTypeInfoAsync_IsCalled : CommonRepositoryBaseTest
    {
        private LoggedInUserTypeInfo _result;
        private List<(long ukprn, bool isProviderActive, bool seedTqProvider)> _testProviderCriteriaData;
        private List<(EnumAwardingOrganisation awardingOrganisation, bool isAoActive, bool seedTqAo, List<(long ukprn, bool isProviderActive, bool seedTqProvider)> providers)> _testCriteriaData;

        public override void Given()
        {
            _testProviderCriteriaData = new List<(long ukprn, bool isProviderActive, bool seedTqProvider)>
            {
                (10000536, true, true), // Barnsley College
                (10000721, true, false), // Bishop Burton College
                (10007315, false, true) // Walsall College                
            };

            _testCriteriaData = new List<(EnumAwardingOrganisation awardingOrganisation, bool isAoActive, bool seedTqAo, List<(long ukprn, bool isProviderActive, bool seedTqProvider)> providers)>
            {
                (EnumAwardingOrganisation.Pearson, true, true, _testProviderCriteriaData),
                (EnumAwardingOrganisation.Ncfe, false, true, _testProviderCriteriaData)
            };

            foreach(var (awardingOrganisation, isAoActive, seedTqAo, providers) in _testCriteriaData)
            {
                SeedTestData(awardingOrganisation, isAoActive, seedTqAo, providers);
            }
            
            CommonRepository = new CommonRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long ukprn)
        {
            if (_result != null)
                return;

            _result = await CommonRepository.GetLoggedInUserTypeInfoAsync(ukprn);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long ukprn, string name, LoginUserType? userType, bool expectedResponse)
        {
            await WhenAsync(ukprn);

            if (_result == null)
            {
                expectedResponse.Should().BeFalse();
                return;
            }

            _result.Ukprn.Should().Be(ukprn);
            _result.Name.Should().Be(name);
            _result.UserType.Should().Be(userType);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Pearson Allowed Awarding Organisation
                    new object[] { 10011881, "Pearson", LoginUserType.AwardingOrganisation, true },

                    // NCFE not allowed Awarding Organisation (as this is not seeding in database)
                    new object[] { 10009696, null, null, false },
                    
                    // Barnsley College is Approved Provider in database
                    new object[] { 10000536, "Barnsley College", LoginUserType.TrainingProvider, true }, 

                    // Bishop Burton College is not Approved Provider in database
                    new object[] { 10000721, null, null, false },

                    // Walsall College is Approved Provider in database but is not Active
                    new object[] { 10007315, null, null, false },

                    // Unknown ukprn number
                    new object[] { 00000000, null, null, false },
                };
            }
        }
    }
}
