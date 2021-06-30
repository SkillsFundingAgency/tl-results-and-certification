using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.PostResultsServiceTests
{
    public class When_FindPrsLearnerRecordAsync_IsCalled : PostResultsServiceRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private FindPrsLearnerRecord _actualResult;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active }
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            foreach (var uln in _ulns)
            {
                _profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, TqProvider));
            }

            TransferRegistration(_profiles.FirstOrDefault(p => p.UniqueLearnerNumber == 1111111113), Provider.WalsallCollege);
            DbContext.SaveChanges();

            // Test class.
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            _actualResult = await PostResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, AwardingOrganisation ao, bool isRecordFound)
        {
            await WhenAsync((long)ao, uln);

            if (isRecordFound == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedProfile = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            _actualResult.Should().NotBeNull();
            _actualResult.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
            _actualResult.Firstname.Should().Be(expectedProfile.Firstname);
            _actualResult.Lastname.Should().Be(expectedProfile.Lastname);
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);

            var expectedPathway = expectedProfile.TqRegistrationPathways.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Status == RegistrationPathwayStatus.Active || x.Status == RegistrationPathwayStatus.Withdrawn);
            _actualResult.Status.Should().Be(expectedPathway.Status);

            var expectedTqAwardingOrganisation = expectedPathway.TqProvider.TqAwardingOrganisation;
            _actualResult.TlevelTitle.Should().Be(expectedTqAwardingOrganisation.TlPathway.TlevelTitle);

            var expctedProvider = expectedTqAwardingOrganisation.TqProviders.FirstOrDefault().TlProvider;

            if (uln == 1111111113)
            {
                _actualResult.ProviderUkprn.Should().Be((int)Provider.WalsallCollege);
                _actualResult.ProviderName.Should().Be("Walsall College");
            }
            else 
            {
                _actualResult.ProviderUkprn.Should().Be(expctedProvider.UkPrn);
                _actualResult.ProviderName.Should().Be(expctedProvider.Name);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, AwardingOrganisation.Pearson, false }, // Invalid Uln
                    new object[] { 1111111111, AwardingOrganisation.Pearson, true }, // Active 
                    new object[] { 1111111111, AwardingOrganisation.Ncfe, false },
                    new object[] { 1111111112, AwardingOrganisation.Pearson, true }, // Withdrawn
                    new object[] { 1111111113, AwardingOrganisation.Pearson, true } 
                };
            }
        }
    }
}
