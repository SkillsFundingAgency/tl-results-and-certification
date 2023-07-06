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
    public class When_GetOverallSpecialismResult_With_Dual_Speciality_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private List<TqRegistrationProfile> _registrations;
        private OverallSpecialismResultDetail _actualResult;

        private static Dictionary<long, (string FirstSpecialismGrade, string SecondSpecialismGrade, string ExpectedOverallSpecialismGrade)> LearnerGradesDict = new()
        {
            { 1111111111, ("Distinction", "Distinction", "Distinction") },
            { 1111111112, ("Distinction", "Merit", "Distinction") },
            { 1111111113, ("Distinction", "Pass", "Merit") },
            { 1111111114, ("Distinction", "Unclassified", "Unclassified") },
            { 1111111115, ("Distinction", null, null) },
            { 1111111116, ("Distinction", "Q - pending result", "Q - pending result") },
            { 1111111117, ("Distinction", "X - no result", "X - no result") },

            { 1111111118, ("Merit", "Distinction", "Distinction") },
            { 1111111119, ("Merit", "Merit", "Merit") },
            { 1111111120, ("Merit", "Pass", "Pass") },
            { 1111111121, ("Merit", "Unclassified", "Unclassified") },
            { 1111111122, ("Merit", null, null) },
            { 1111111123, ("Merit", "Q - pending result", "Q - pending result") },
            { 1111111124, ("Merit", "X - no result", "X - no result") },

            { 1111111125, ("Pass", "Distinction", "Merit") },
            { 1111111126, ("Pass", "Merit", "Pass") },
            { 1111111127, ("Pass", "Pass", "Pass") },
            { 1111111128, ("Pass", "Unclassified", "Unclassified") },
            { 1111111129, ("Pass", null, null) },
            { 1111111130, ("Pass", "Q - pending result", "Q - pending result") },
            { 1111111131, ("Pass", "X - no result", "X - no result") },

            { 1111111132, ("Unclassified", "Distinction", "Unclassified") },
            { 1111111133, ("Unclassified", "Merit", "Unclassified") },
            { 1111111134, ("Unclassified", "Pass", "Unclassified") },
            { 1111111135, ("Unclassified", "Unclassified", "Unclassified") },
            { 1111111136, ("Unclassified", null, null) },
            { 1111111137, ("Unclassified", "Q - pending result", "Q - pending result") },
            { 1111111138, ("Unclassified", "X - no result", "X - no result") },

            { 1111111139, (null, "Distinction", null) },
            { 1111111140, (null, "Merit", null) },
            { 1111111141, (null, "Pass", null) },
            { 1111111142, (null, "Unclassified", null) },
            { 1111111143, (null, null, null) },
            { 1111111144, (null, "Q - pending result", "Q - pending result") },
            { 1111111145, (null, "X - no result", "X - no result") },

            { 1111111146, ("Q - pending result", "Distinction", "Q - pending result") },
            { 1111111147, ("Q - pending result", "Merit", "Q - pending result") },
            { 1111111148, ("Q - pending result", "Pass", "Q - pending result") },
            { 1111111149, ("Q - pending result", "Unclassified", "Q - pending result") },
            { 1111111150, ("Q - pending result", null, "Q - pending result") },
            { 1111111151, ("Q - pending result", "Q - pending result", "Q - pending result") },
            { 1111111152, ("Q - pending result", "X - no result", "X - no result") },

            { 1111111153, ("X - no result", "Distinction", "X - no result") },
            { 1111111154, ("X - no result", "Merit", "X - no result") },
            { 1111111155, ("X - no result", "Pass", "X - no result") },
            { 1111111156, ("X - no result", "Unclassified", "X - no result") },
            { 1111111157, ("X - no result", null, "X - no result") },
            { 1111111158, ("X - no result", "Q - pending result", "X - no result") },
            { 1111111159, ("X - no result", "X - no result", "X - no result") }
        };

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedDualSpecialismRegistrationData(LearnerGradesDict.Keys);

            int currentAcademicYear = GetAcademicYear();
            _registrations.ForEach(x => x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1));

            foreach (var currentLearnerGrades in LearnerGradesDict)
            {
                SeedAssesmentsResults(_registrations, currentLearnerGrades.Key, new[] { currentLearnerGrades.Value.FirstSpecialismGrade, currentLearnerGrades.Value.SecondSpecialismGrade }, $"Summer {currentAcademicYear}");
            }

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
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, string expectedGrade)
        {
            var pathway = _registrations.SelectMany(r => r.TqRegistrationPathways).FirstOrDefault(r => r.TqRegistrationProfile.UniqueLearnerNumber == uln);

            var specialisms = pathway.TqRegistrationSpecialisms;
            await WhenAsync(specialisms);

            OverallSpecialismResultDetail expectedResult = CreateExpectedOverallSpecialismResultDetail(specialisms, expectedGrade);
            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return LearnerGradesDict.Select(p => new object[] { p.Key, p.Value.ExpectedOverallSpecialismGrade });
            }
        }

        private OverallSpecialismResultDetail CreateExpectedOverallSpecialismResultDetail(IEnumerable<TqRegistrationSpecialism> registrationSpecialisms, string expectedGrade)
        {
            TqRegistrationSpecialism firstRegistrationSpecialism = registrationSpecialisms.FirstOrDefault();
            TqRegistrationSpecialism secondRegistrationSpecialism = registrationSpecialisms.LastOrDefault();

            return new OverallSpecialismResultDetail()
            {
                SpecialismDetails = new List<OverallSpecialismDetail>
                {
                    CreateOverallSpecialismDetail(firstRegistrationSpecialism),
                    CreateOverallSpecialismDetail(secondRegistrationSpecialism)
                },
                TlLookupId = TlLookup.SingleOrDefault(x => x.Category == "SpecialismComponentGrade" && x.Value == expectedGrade)?.Id,
                OverallSpecialismResult = expectedGrade
            };
        }

        private OverallSpecialismDetail CreateOverallSpecialismDetail(TqRegistrationSpecialism registrationSpecialism)
        {
            TlSpecialism firstSpecialism = registrationSpecialism?.TlSpecialism;
            string firstSpecialismResult = registrationSpecialism?.TqSpecialismAssessments.FirstOrDefault()?.TqSpecialismResults.FirstOrDefault()?.TlLookup?.Value;

            return new OverallSpecialismDetail
            {
                SpecialismName = firstSpecialism?.Name,
                SpecialismLarId = firstSpecialism?.LarId,
                SpecialismResult = firstSpecialismResult
            };
        }
    }
}