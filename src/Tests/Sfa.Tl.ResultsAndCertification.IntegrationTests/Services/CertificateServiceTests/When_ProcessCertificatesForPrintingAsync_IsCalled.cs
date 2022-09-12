using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests
{
    public class When_ProcessCertificatesForPrintingAsync_IsCalled : CertificateServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<OverallResult> _expectedResults = new List<OverallResult>();
        private List<CertificateResponse> _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },    // Valid (Barnsley College)
                { 1111111112, RegistrationPathwayStatus.Active },    // Valid (Barnsley College)
                { 1111111113, RegistrationPathwayStatus.Active },    // Valid (Bishop Burton College)
                { 1111111114, RegistrationPathwayStatus.Active },    // Valid (Bishop Burton College)
                { 1111111115, RegistrationPathwayStatus.Active },    // Valid (Wallsal)
                { 1111111116, RegistrationPathwayStatus.Active }     // Valid (Walsall)
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            SeedTqProvider(Provider.BishopBurtonCollege);
            SetRegistrationProviders(_registrations, new List<long> { 1111111113, 1111111114 }, Provider.BishopBurtonCollege);

            SeedTqProvider(Provider.WalsallCollege);
            SetRegistrationProviders(_registrations, new List<long> { 1111111115, 1111111116 }, Provider.WalsallCollege);

            // Seed Overall results for all learners
            foreach (var registration in _registrations)
            {
                _expectedResults.Add(new OverallResultCustomBuilder()
                    .WithTqRegistrationPathwayId(GetPathwayId(registration.UniqueLearnerNumber))
                    .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                    .WithCalculationStatus(CalculationStatus.Completed)
                    .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                    .Save(DbContext));
            }

            // Create CertificateService
            CreateService();
        }

        public override async Task When()
        {
            _actualResult = await CertificateService.ProcessCertificatesForPrintingAsync();
        }

        [Fact]
        public void Then_ExpectedResults_Are_Returned()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(2);

            var expectedResult = new List<CertificateResponse>
            {
                new CertificateResponse {IsSuccess = true, BatchId = 1, ProvidersCount = 2, CertificatesCreated = 4 },
                new CertificateResponse {IsSuccess = true, BatchId = 2, ProvidersCount = 1, CertificatesCreated = 2 },
            };

            _actualResult.Should().BeEquivalentTo(expectedResult);

            foreach (var registration in _registrations)
            {
                var expectedPerformedBy = "System";
                var expectedProfile = registration;
                var expectedPathway = expectedProfile.TqRegistrationPathways.FirstOrDefault();
                var expectedProviderAddress = expectedPathway.TqProvider.TlProvider.TlProviderAddresses.FirstOrDefault();
                var expectedOverallResult = expectedPathway.OverallResults.FirstOrDefault(o => o.TqRegistrationPathwayId == expectedPathway.Id);

                var expectedLearnerDetails = GetLearnerDetails(expectedOverallResult);

                var actualPrintCertificate = DbContext.PrintCertificate.Where(p => p.Uln == expectedProfile.UniqueLearnerNumber && p.TqRegistrationPathwayId == expectedPathway.Id)
                                                                       .Include(p => p.PrintBatchItem)
                                                                            .ThenInclude(p => p.Batch)
                                                                       .OrderByDescending(p => p.CreatedOn).FirstOrDefault();

                // Assert PrintCertificate
                actualPrintCertificate.Should().NotBeNull();
                actualPrintCertificate.TqRegistrationPathwayId.Should().Be(expectedPathway.Id);
                actualPrintCertificate.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
                actualPrintCertificate.LearnerName.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
                actualPrintCertificate.Type.Should().Be(PrintCertificateType.Certificate);
                actualPrintCertificate.LearningDetails.Should().BeEquivalentTo(expectedLearnerDetails);
                actualPrintCertificate.DisplaySnapshot.Should().BeNull();
                actualPrintCertificate.CreatedBy.Should().Be(expectedPerformedBy);

                // Assert PrintBatchItem
                var actualPrintBatchItem = actualPrintCertificate.PrintBatchItem;
                actualPrintBatchItem.Should().NotBeNull();
                actualPrintBatchItem.TlProviderAddressId.Should().Be(expectedProviderAddress.Id);
                actualPrintBatchItem.CreatedBy.Should().Be(expectedPerformedBy);

                // Assert Batch
                var actualBatch = actualPrintBatchItem.Batch;
                actualBatch.Should().NotBeNull();
                actualBatch.Type.Should().Be(BatchType.Printing);
                actualBatch.Status.Should().Be(BatchStatus.Created);
                actualBatch.CreatedBy.Should().Be(expectedPerformedBy);
            }
        }

        private string GetLearnerDetails(OverallResult overallResult)
        {
            var overallResultDetail = JsonConvert.DeserializeObject<OverallResultDetail>(overallResult.Details);

            var specialisms = new List<OccupationalSpecialism>();
            if (overallResultDetail.SpecialismDetails == null || !overallResultDetail.SpecialismDetails.Any())
                specialisms.Add(new OccupationalSpecialism { Specialism = string.Empty, Grade = Constants.NotCompleted });
            else
            {
                overallResultDetail.SpecialismDetails.ForEach(x =>
                {
                    specialisms.Add(new OccupationalSpecialism { Specialism = x.SpecialismName, Grade = !string.IsNullOrWhiteSpace(x.SpecialismResult) ? x.SpecialismResult : Constants.NotCompleted });
                });
            }

            var learningDetails = new LearningDetails
            {
                TLevelTitle = overallResultDetail.TlevelTitle.Replace(Constants.TLevelIn, string.Empty, StringComparison.InvariantCultureIgnoreCase),
                Core = overallResultDetail.PathwayName,
                CoreGrade = !string.IsNullOrWhiteSpace(overallResultDetail.PathwayResult) ? overallResultDetail.PathwayResult : Constants.NotCompleted,
                OccupationalSpecialism = specialisms,
                IndustryPlacement = GetIndustryPlacementText(overallResultDetail.IndustryPlacementStatus),
                Grade = overallResult.ResultAwarded,
                EnglishAndMaths = GetEnglishAndMathsText(overallResult.TqRegistrationPathway.TqRegistrationProfile.EnglishStatus, overallResult.TqRegistrationPathway.TqRegistrationProfile.MathsStatus),
                Date = DateTime.UtcNow.ToCertificateDateFormat()
            };

            return JsonConvert.SerializeObject(learningDetails);
        }

        private string GetEnglishAndMathsText(SubjectStatus? englishStatus, SubjectStatus? mathsStatus)
        {
            if ((englishStatus == SubjectStatus.Achieved || englishStatus == SubjectStatus.AchievedByLrs) &&
                (mathsStatus == SubjectStatus.Achieved || mathsStatus == SubjectStatus.AchievedByLrs))
                return "The named recipient has also achieved a qualification at Level 2 in both maths and English.";
            else if (mathsStatus == SubjectStatus.Achieved || mathsStatus == SubjectStatus.AchievedByLrs)
                return "The named recipient has also achieved a qualification at Level 2 in maths.";
            else if (englishStatus == SubjectStatus.Achieved || englishStatus == SubjectStatus.AchievedByLrs)
                return "The named recipient has also achieved a qualification at Level 2 in English.";
            else
                return null;
        }

        private static string GetIndustryPlacementText(string industryPlacementStatus)
        {
            var ipStatus = EnumExtensions.GetEnumByDisplayName<IndustryPlacementStatus>(industryPlacementStatus);

            if (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return "Met";

            return "Not met";
        }

        private int GetPathwayId(long uln)
        {
            return _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
        }
    }
}
