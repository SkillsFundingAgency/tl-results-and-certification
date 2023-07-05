using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_DownloadOverallResultsData_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IList<DownloadOverallResultsData> _actualResult;
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<long> _ulnsAlreadyWithCalculatedResult;
        private List<OverallGradeLookup> _overallGradeLookup;
        private readonly string _previousAssessmentName = "Previous Assessment";

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn }
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in _registrations)
            {
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList());
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Build Pathway results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments));

                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList());
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessments));
            }

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            var pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            var specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            DbContext.SaveChanges();

            // Seed OverallResultLookup
            _ulnsAlreadyWithCalculatedResult = new List<long> { 1111111111, 1111111112 };
            _overallGradeLookup = new List<OverallGradeLookup>();
            foreach (var uln in _ulnsAlreadyWithCalculatedResult)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
                var coreResultId = pathwayResults.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
                var splResultId = specialismResults.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 3, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
            }
            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

            // Seed Overall results
            var publishDate = DateTime.Today.AddDays(-1);
            var printAvailableFrom = DateTime.Now.Date.AddMonths(5).AddDays(1);

            var regPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111111).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = regPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.Certificate, PrintAvailableFrom = printAvailableFrom, PublishDate = publishDate } }, true);

            // WithDrawn overall result.
            regPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111112).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = regPathwayId,
                Details = "{Merit}", ResultAwarded = "Merit", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.Certificate, PrintAvailableFrom = printAvailableFrom, PublishDate = publishDate } }, true);

            // SeedPreviousAssessment
            var prevAssessmentSeries = new AssessmentSeries { Name = _previousAssessmentName, Year = 2020, StartDate = DateTime.Today.AddMonths(-3), EndDate = DateTime.Today.AddDays(-2), ResultPublishDate = publishDate, ComponentType = ComponentType.Core, ResultCalculationYear = 2020 };
            AssessmentSeriesDataProvider.CreateAssessmentSeries(DbContext, prevAssessmentSeries, true);

            DbContext.SaveChanges();
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, DateTime assessmentPublishDate)
        {
            var prevAssessment = DbContext.AssessmentSeries.FirstOrDefault(x => x.Name.Equals(_previousAssessmentName, StringComparison.InvariantCultureIgnoreCase));
            prevAssessment.ResultPublishDate = assessmentPublishDate;
            DbContext.SaveChanges();

            _actualResult = await OverallResultCalculationService.DownloadOverallResultsDataAsync(providerUkprn);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long providerUkprn, DateTime assessmentPublishDate, List<long> expectedUlns)
        {
            await WhenAsync(providerUkprn, assessmentPublishDate);

            if (!expectedUlns.Any())
            {
                _actualResult.Count.Should().Be(0);
                return;
            }

            _actualResult.Count.Should().Be(expectedUlns.Count);
            foreach (var expectedUln in expectedUlns)
            {
                var actualResult = _actualResult.FirstOrDefault(x => x.Uln == expectedUln);
                var expectedResult = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == expectedUln);

                actualResult.Should().NotBeNull();
                actualResult.Uln.Should().Be(expectedResult.UniqueLearnerNumber);
                actualResult.LastName.Should().Be(expectedResult.Lastname);
                actualResult.FirstName.Should().Be(expectedResult.Firstname);
                actualResult.DisplayDateOfBirth.Should().Be(expectedResult.DateofBirth.ToString("dd-MMM-yyyy"));
                actualResult.DisplayStartYear.Should().Be($"{expectedResult.TqRegistrationPathways.FirstOrDefault().AcademicYear} to {expectedResult.TqRegistrationPathways.FirstOrDefault().AcademicYear + 1}");

                var expectedOverallResults = expectedResult.TqRegistrationPathways.FirstOrDefault().OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                var expectedOverallResultsDetails = JsonConvert.DeserializeObject<OverallResultDetail>(expectedOverallResults.Details);
                actualResult.Tlevel.Should().Be($"\"{expectedOverallResultsDetails.TlevelTitle}\"");
                actualResult.CoreCode.Should().Be($"{expectedOverallResultsDetails.PathwayLarId}");
                actualResult.CoreComponent.Should().Be($"\"{expectedOverallResultsDetails.PathwayName}\"");
                actualResult.CoreResult.Should().Be($"{expectedOverallResultsDetails.PathwayResult}");

                actualResult.SpecialismCode.Should().Be($"{expectedOverallResultsDetails.SpecialismDetails.FirstOrDefault().SpecialismLarId}");
                actualResult.SpecialismComponent.Should().Be($"\"{expectedOverallResultsDetails.SpecialismDetails.FirstOrDefault().SpecialismName}\"");
                actualResult.SpecialismResult.Should().Be(expectedOverallResults.SpecialismResultAwarded);

                actualResult.IndustryPlacementStatus.Should().Be($"{expectedOverallResultsDetails.IndustryPlacementStatus}");
                actualResult.OverallResult.Should().Be(expectedOverallResults.ResultAwarded);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { (long)Provider.BarsleyCollege, DateTime.Today.AddDays(-1), new List<long> { 1111111111 } }, // valid
                    new object[] { (long)Provider.BarsleyCollege, DateTime.Today.AddDays(1), new List<long>() },  // Publishdate not reached 
                    new object[] { (long)Provider.WalsallCollege, DateTime.Today.AddDays(-1), new List<long>() }  // Different Proivder
                };
            }
        }
    }
}
