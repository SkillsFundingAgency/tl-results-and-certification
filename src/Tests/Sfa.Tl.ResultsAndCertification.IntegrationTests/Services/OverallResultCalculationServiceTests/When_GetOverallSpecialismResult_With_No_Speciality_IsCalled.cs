using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetOverallSpecialismResult_With_No_Speciality_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private List<TqRegistrationProfile> _registrations;
        private OverallSpecialismResultDetail _actualResult;

        public override void Given()
        {
            var ulns = new long[] { 1111111111, 1111111112 };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedNoSpecialismRegistrationData(ulns);

            var currentAcademicYear = GetAcademicYear();

            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            DbContext.SaveChanges();
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ICollection<TqRegistrationSpecialism> specialisms)
        {
            _actualResult = await OverallResultCalculationService.GetOverallSpecialismResult(TlLookup, specialisms);
            await Task.CompletedTask;
        }

        [Theory]
        [InlineData(1111111111)]
        [InlineData(1111111112)]
        public async Task Then_Expected_Results_Are_Returned(long uln)
        {
            var pathway = _registrations.SelectMany(r => r.TqRegistrationPathways).FirstOrDefault(r => r.TqRegistrationProfile.UniqueLearnerNumber == uln);

            var specialisms = pathway.TqRegistrationSpecialisms;
            await WhenAsync(specialisms);

            OverallSpecialismResultDetail expectedResult = CreateExpectedOverallSpecialismResultDetail();
            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private OverallSpecialismResultDetail CreateExpectedOverallSpecialismResultDetail()
        {
            return new OverallSpecialismResultDetail()
            {
                SpecialismDetails = new List<OverallSpecialismDetail>(),
                TlLookupId = null,
                OverallSpecialismResult = null
            };
        }
    }
}
