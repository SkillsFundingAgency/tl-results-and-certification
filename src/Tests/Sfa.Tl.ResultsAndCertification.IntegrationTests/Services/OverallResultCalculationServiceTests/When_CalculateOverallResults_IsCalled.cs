using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_CalculateOverallResults_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private const int PathwayComponentGradeAPlus = 1;
        private const int SpecialismComponentGradeDistinction = 10;
        private const int OverallResultGradeDistinctionPlus = 17;

        private Dictionary<string, AssessmentSeries> _coreAssessmentSeries;

        private List<OverallResultResponse> _results;
        private List<OverallResultResponse> _expectedResults = new()
        {
            new OverallResultResponse
            {
                IsSuccess = true,
                TotalRecords = 2,
                NewRecords = 2,
                UpdatedRecords = 0,
                UnChangedRecords = 0,
                SavedRecords = 2
            },
            new OverallResultResponse
            {
                IsSuccess = true,
                TotalRecords = 2,
                NewRecords = 0,
                UpdatedRecords = 2,
                UnChangedRecords = 0,
                SavedRecords = 4
            },
            new OverallResultResponse
            {
                IsSuccess = true,
                TotalRecords = 1,
                NewRecords = 0,
                UpdatedRecords = 0,
                UnChangedRecords = 1,
                SavedRecords = 0
            }
        };

        public override void Given()
        {
            const int BarnsleyCollegeUkprn = 10000536;

            _coreAssessmentSeries = SeedCoreAssessmentSeries();
            Dictionary<string, AssessmentSeries> specialismAssessmentSeries = SeedSpecialismAssessmentSeries();

            TqAwardingOrganisation pearson = SeedAwardingOrganisation(EnumAwardingOrganisation.Pearson);
            TqProvider barnsleyCollege = SeedProvider(pearson, BarnsleyCollegeUkprn, "Barnsley College");

            Dictionary<string, TlSpecialism> specialisms = SeedSpecialisms(pearson.TlPathway);

            // No results. Generates a new overall result.
            TqRegistrationProfile johnSmith = SeedRegistrationProfile(1111111111, "John", "Smith", new DateTime(2000, 1, 1));
            SeedRegistrationPathway(johnSmith, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);

            // Has results and no overall result. Generates a new overall result.
            TqRegistrationProfile graceBaker = SeedRegistrationProfile(1111111112, "Grace", "Baker", new DateTime(2005, 8, 25));
            TqRegistrationPathway graceBakerRegistration = SeedRegistrationPathway(graceBaker, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedIndustryPlacement(graceBakerRegistration, new DateTime(2024, 4, 1), new DateTime(2024, 6, 2));
            TqPathwayAssessment graceBakerPathwayAssessment = SeedPathwayAssessment(graceBakerRegistration, _coreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(graceBakerPathwayAssessment, PathwayComponentGradeAPlus, new DateTime(2024, 6, 12), new DateTime(2024, 6, 12));
            TqRegistrationSpecialism graceBakerRegistrationSpecialism = SeedRegistrationSpecialism(graceBakerRegistration, specialisms["Civil Engineering"]);
            TqSpecialismAssessment graceBakerSpecialismAssessment = SeedSpecialismAssessment(graceBakerRegistrationSpecialism, specialismAssessmentSeries["Summer 2024"]);
            SeedSpecialismResult(graceBakerSpecialismAssessment, SpecialismComponentGradeDistinction, new DateTime(2024, 6, 15), new DateTime(2024, 6, 15));

            // No results and a existing overall result. The overall result is updated (different outcome).
            TqRegistrationProfile jessicaRoberts = SeedRegistrationProfile(1111111113, "Jessica", "Roberts", new DateTime(2004, 9, 5));
            TqRegistrationPathway jessicaRobertsRegistration = SeedRegistrationPathway(jessicaRoberts, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(jessicaRobertsRegistration, "Distinction", "Distinction", new DateTime(2024, 10, 15), @"{""TlevelTitle"":""T Level in Design, Surveying and Planning for Construction"",""PathwayName"":""Design, Surveying and Planning"",""PathwayLarId"":""10123456"",""PathwayResult"":""B"",""SpecialismDetails"":[{""SpecialismName"":""Civil Engineering"",""SpecialismLarId"":""ZTLOS002"",""SpecialismResult"":""Pass""}],""IndustryPlacementStatus"":""Completed"",""OverallResult"":""Distinction""}");
            SeedIndustryPlacement(jessicaRobertsRegistration, new DateTime(2024, 10, 16), new DateTime(2024, 10, 16));

            // Has results and a existing overall result. The overall result is updated (different outcome).
            TqRegistrationProfile lucyWalker = SeedRegistrationProfile(1111111114, "Lucy", "Walker", new DateTime(2005, 8, 25));
            TqRegistrationPathway lucyWalkerRegistration = SeedRegistrationPathway(lucyWalker, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(lucyWalkerRegistration, "Merit", "Pass", new DateTime(2024, 6, 1), @"{""TlevelTitle"":""T Level in Design, Surveying and Planning for Construction"",""PathwayName"":""Design, Surveying and Planning"",""PathwayLarId"":""10123456"",""PathwayResult"":""B"",""SpecialismDetails"":[{""SpecialismName"":""Civil Engineering"",""SpecialismLarId"":""ZTLOS002"",""SpecialismResult"":""Pass""}],""IndustryPlacementStatus"":""Completed"",""OverallResult"":""Merit""}");
            SeedIndustryPlacement(lucyWalkerRegistration, new DateTime(2024, 4, 1), new DateTime(2024, 6, 1, 10, 0, 0));
            TqPathwayAssessment lucyWalkerPathwayAssessment = SeedPathwayAssessment(lucyWalkerRegistration, _coreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(lucyWalkerPathwayAssessment, PathwayComponentGradeAPlus, new DateTime(2024, 6, 12), new DateTime(2024, 6, 12));
            TqRegistrationSpecialism lucyWalkerRegistrationSpecialism = SeedRegistrationSpecialism(lucyWalkerRegistration, specialisms["Civil Engineering"]);
            TqSpecialismAssessment lucyWalkerSpecialismAssessment = SeedSpecialismAssessment(lucyWalkerRegistrationSpecialism, specialismAssessmentSeries["Summer 2024"]);
            SeedSpecialismResult(lucyWalkerSpecialismAssessment, SpecialismComponentGradeDistinction, new DateTime(2024, 6, 15), new DateTime(2024, 6, 15));

            // Has results and a existing overall result. The overall result is not updated (same outcome).
            TqRegistrationProfile alexJohnson = SeedRegistrationProfile(1111111115, "Alex", "Johnson", new DateTime(2005, 8, 25));
            TqRegistrationPathway alexJohnsonRegistration = SeedRegistrationPathway(alexJohnson, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(alexJohnsonRegistration, "Distinction*", "Distinction", new DateTime(2024, 6, 1), @"{""TlevelTitle"":""T Level in Design, Surveying and Planning for Construction"",""PathwayName"":""Design, Surveying and Planning"",""PathwayLarId"":""10123456"",""PathwayResult"":""A*"",""SpecialismDetails"":[{""SpecialismName"":""Civil Engineering"",""SpecialismLarId"":""ZTLOS002"",""SpecialismResult"":""Distinction""}],""IndustryPlacementStatus"":""Completed"",""OverallResult"":""Distinction*""}");
            SeedIndustryPlacement(alexJohnsonRegistration, new DateTime(2024, 4, 1), new DateTime(2024, 6, 1, 10, 0, 0));
            TqPathwayAssessment alexJohnsonPathwayAssessment = SeedPathwayAssessment(alexJohnsonRegistration, _coreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(alexJohnsonPathwayAssessment, PathwayComponentGradeAPlus, new DateTime(2024, 6, 12), new DateTime(2024, 6, 12));
            TqRegistrationSpecialism alexJohnsonRegistrationSpecialism = SeedRegistrationSpecialism(alexJohnsonRegistration, specialisms["Civil Engineering"]);
            TqSpecialismAssessment alexJohnsonSpecialismAssessment = SeedSpecialismAssessment(alexJohnsonRegistrationSpecialism, specialismAssessmentSeries["Summer 2024"]);
            SeedSpecialismResult(alexJohnsonSpecialismAssessment, SpecialismComponentGradeDistinction, new DateTime(2024, 6, 15), new DateTime(2024, 6, 15));

            TlLookupDataProvider.CreateFullTlLookupList(DbContext);
            SeedOverallGradeLookups(barnsleyCollege.TqAwardingOrganisation.TlPathway);

            DbContext.SaveChanges();
            CreateService();
        }

        public override async Task When()
        {
            DateTime runDate = new(2025, 3, 11);
            _results = await OverallResultCalculationService.CalculateOverallResultsAsync(runDate);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            _results.Should().BeEquivalentTo(_expectedResults);

            DateTime resultPublishDate = _coreAssessmentSeries["Autumn 2024"].ResultPublishDate.Value;

            List<OverallResult> overallResults1 = await GetOverallResultsByUln(1111111111);
            overallResults1.Should().ContainSingle();
            overallResults1[0].ResultAwarded.Should().Be("X - no result");
            overallResults1[0].SpecialismResultAwarded.Should().BeNull();
            overallResults1[0].CalculationStatus.Should().Be(CalculationStatus.NoResult);
            overallResults1[0].PublishDate.Should().Be(resultPublishDate);
            overallResults1[0].IsOptedin.Should().BeTrue();
            overallResults1[0].EndDate.Should().NotHaveValue();
            overallResults1[0].ModifiedBy.Should().BeNull();
            overallResults1[0].ModifiedOn.Should().NotHaveValue();

            List<OverallResult> overallResults2 = await GetOverallResultsByUln(1111111112);
            overallResults2.Should().ContainSingle();
            overallResults2[0].ResultAwarded.Should().Be("Distinction*");
            overallResults2[0].SpecialismResultAwarded.Should().Be("Distinction");
            overallResults2[0].CalculationStatus.Should().Be(CalculationStatus.Completed);
            overallResults2[0].PublishDate.Should().Be(resultPublishDate);
            overallResults2[0].IsOptedin.Should().BeTrue();
            overallResults2[0].EndDate.Should().NotHaveValue();
            overallResults2[0].ModifiedBy.Should().BeNull();
            overallResults2[0].ModifiedOn.Should().NotHaveValue();

            List<OverallResult> overallResults3 = await GetOverallResultsByUln(1111111113);
            overallResults3.Should().HaveCount(2);
            overallResults3[0].ResultAwarded.Should().Be("Distinction");
            overallResults3[0].SpecialismResultAwarded.Should().Be("Distinction");
            overallResults3[0].CalculationStatus.Should().Be(CalculationStatus.Completed);
            overallResults3[0].PublishDate.Should().NotHaveValue();
            overallResults3[0].IsOptedin.Should().BeFalse();
            overallResults3[0].EndDate.Should().HaveValue();
            overallResults3[0].ModifiedBy.Should().NotBeNullOrEmpty();
            overallResults3[0].ModifiedOn.Should().HaveValue();
            overallResults3[1].ResultAwarded.Should().Be("Partial achievement");
            overallResults3[1].SpecialismResultAwarded.Should().BeNull();
            overallResults3[1].CalculationStatus.Should().Be(CalculationStatus.PartiallyCompleted);
            overallResults3[1].PublishDate.Should().Be(resultPublishDate);
            overallResults3[1].IsOptedin.Should().BeTrue();
            overallResults3[1].EndDate.Should().NotHaveValue();
            overallResults3[1].ModifiedBy.Should().BeNull();
            overallResults3[1].ModifiedOn.Should().NotHaveValue();

            List<OverallResult> overallResults4 = await GetOverallResultsByUln(1111111114);
            overallResults4.Should().HaveCount(2);
            overallResults4[0].ResultAwarded.Should().Be("Merit");
            overallResults4[0].SpecialismResultAwarded.Should().Be("Pass");
            overallResults4[0].CalculationStatus.Should().Be(CalculationStatus.Completed);
            overallResults4[0].PublishDate.Should().NotHaveValue();
            overallResults4[0].IsOptedin.Should().BeFalse();
            overallResults4[0].EndDate.Should().HaveValue();
            overallResults4[0].ModifiedBy.Should().NotBeNullOrEmpty();
            overallResults4[0].ModifiedOn.Should().HaveValue();
            overallResults4[1].ResultAwarded.Should().Be("Distinction*");
            overallResults4[1].SpecialismResultAwarded.Should().Be("Distinction");
            overallResults4[1].CalculationStatus.Should().Be(CalculationStatus.Completed);
            overallResults4[1].PublishDate.Should().Be(resultPublishDate);
            overallResults4[1].IsOptedin.Should().BeTrue();
            overallResults4[1].EndDate.Should().NotHaveValue();
            overallResults4[1].ModifiedBy.Should().BeNull();
            overallResults4[1].ModifiedOn.Should().NotHaveValue();

            List<OverallResult> overallResults5 = await GetOverallResultsByUln(1111111115);
            overallResults5.Should().ContainSingle();
            overallResults5[0].ResultAwarded.Should().Be("Distinction*");
            overallResults5[0].SpecialismResultAwarded.Should().Be("Distinction");
            overallResults5[0].CalculationStatus.Should().Be(CalculationStatus.Completed);
            overallResults5[0].PublishDate.Should().NotHaveValue();
            overallResults5[0].IsOptedin.Should().BeTrue();
            overallResults5[0].EndDate.Should().NotHaveValue();
            overallResults5[0].ModifiedBy.Should().BeNull();
            overallResults5[0].ModifiedOn.Should().NotHaveValue();

            async Task<List<OverallResult>> GetOverallResultsByUln(long uln)
                => await DbContext.OverallResult.Where(r => r.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln).ToListAsync();
        }

        #region Seed methods

        private TqAwardingOrganisation SeedAwardingOrganisation(EnumAwardingOrganisation awardingOrganisation)
        {
            TlAwardingOrganisation tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            TlRoute route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            TlPathway pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, route);
            TqAwardingOrganisation tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, pathway, tlAwardingOrganisation);

            return tqAwardingOrganisation;
        }

        private TqProvider SeedProvider(TqAwardingOrganisation tqAwardingOrganisation, long providerUkprn, string providerName)
        {
            var tlProvider = ProviderDataProvider.CreateTlProvider(DbContext, providerUkprn, providerName, providerName);
            var tqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider);

            return tqProvider;
        }

        private TqRegistrationProfile SeedRegistrationProfile(long uln, string firstName, string lastName, DateTime dob)
        {
            var tqRegistrationProfile = new TqRegistrationProfile
            {
                UniqueLearnerNumber = uln,
                Firstname = firstName,
                Lastname = lastName,
                DateofBirth = dob
            };

            return RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, tqRegistrationProfile);
        }

        private TqRegistrationPathway SeedRegistrationPathway(TqRegistrationProfile tqRegistrationProfile, TqProvider tqProvider, int academicYear, RegistrationPathwayStatus status)
        {
            var tqRegistrationPathway = new TqRegistrationPathway
            {
                TqRegistrationProfileId = tqRegistrationProfile.Id,
                TqProviderId = tqProvider.Id,
                AcademicYear = academicYear,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                Status = status
            };

            return RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationPathway);
        }

        private Dictionary<string, AssessmentSeries> SeedCoreAssessmentSeries()
        {
            var summer2022 = new AssessmentSeries
            {
                Id = 3,
                ComponentType = ComponentType.Core,
                Name = "Summer 2022",
                Description = "Summer 2022",
                Year = 2022,
                StartDate = new DateTime(2022, 3, 8),
                EndDate = new DateTime(2022, 8, 8),
                ResultCalculationYear = 2020,
                ResultPublishDate = new DateTime(2022, 8, 17)
            };

            var autumn2022 = new AssessmentSeries
            {
                Id = 4,
                ComponentType = ComponentType.Core,
                Name = "Autumn 2022",
                Description = "Autumn 2022",
                Year = 2022,
                StartDate = new DateTime(2022, 8, 9),
                EndDate = new DateTime(2023, 3, 6),
                ResultCalculationYear = 2020,
                ResultPublishDate = new DateTime(2023, 3, 15)
            };

            var summer2023 = new AssessmentSeries
            {
                Id = 5,
                ComponentType = ComponentType.Core,
                Name = "Summer 2023",
                Description = "Summer 2023",
                Year = 2023,
                StartDate = new DateTime(2022, 3, 7),
                EndDate = new DateTime(2023, 8, 7),
                ResultCalculationYear = 2021,
                ResultPublishDate = new DateTime(2023, 8, 16)
            };

            var autumn2023 = new AssessmentSeries
            {
                Id = 6,
                ComponentType = ComponentType.Core,
                Name = "Autumn 2023",
                Description = "Autumn 2023",
                Year = 2023,
                StartDate = new DateTime(2022, 8, 8),
                EndDate = new DateTime(2024, 3, 11),
                ResultCalculationYear = 2021,
                ResultPublishDate = new DateTime(2024, 3, 20)
            };

            var summer2024 = new AssessmentSeries
            {
                Id = 10,
                ComponentType = ComponentType.Core,
                Name = "Summer 2024",
                Description = "Summer 2024",
                Year = 2024,
                StartDate = new DateTime(2024, 3, 12),
                EndDate = new DateTime(2024, 8, 5),
                ResultCalculationYear = 2022,
                ResultPublishDate = new DateTime(2024, 8, 14)
            };

            var autumn2024 = new AssessmentSeries
            {
                Id = 11,
                ComponentType = ComponentType.Core,
                Name = "Autumn 2024",
                Description = "Autumn 2024",
                Year = 2024,
                StartDate = new DateTime(2024, 8, 6),
                EndDate = new DateTime(2025, 3, 10),
                ResultCalculationYear = 2022,
                ResultPublishDate = new DateTime(2025, 3, 19)
            };

            var summer2025 = new AssessmentSeries
            {
                Id = 12,
                ComponentType = ComponentType.Core,
                Name = "Summer 2025",
                Description = "Summer 2025",
                Year = 2025,
                StartDate = new DateTime(2025, 3, 11),
                EndDate = new DateTime(2025, 8, 11),
                ResultCalculationYear = 2023,
                ResultPublishDate = new DateTime(2025, 8, 20)
            };

            var assessmentSeries = new List<AssessmentSeries> { summer2022, autumn2022, summer2023, autumn2023, summer2024, autumn2024, summer2025 };
            AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, assessmentSeries);

            return assessmentSeries.ToDictionary(a => a.Name, a => a);
        }

        private Dictionary<string, AssessmentSeries> SeedSpecialismAssessmentSeries()
        {
            var summer2022 = new AssessmentSeries
            {
                Id = 7,
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2022",
                Description = "Summer 2022",
                Year = 2022,
                StartDate = new DateTime(2021, 10, 1),
                EndDate = new DateTime(2022, 8, 8)
            };

            var summer2023 = new AssessmentSeries
            {
                Id = 8,
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2023",
                Description = "Summer 2023",
                Year = 2023,
                StartDate = new DateTime(2022, 8, 9),
                EndDate = new DateTime(2023, 8, 7)
            };

            var summer2024 = new AssessmentSeries
            {
                Id = 9,
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2024",
                Description = "Summer 2024",
                Year = 2024,
                StartDate = new DateTime(2023, 8, 18),
                EndDate = new DateTime(2024, 8, 5)
            };

            var summer2025 = new AssessmentSeries
            {
                Id = 13,
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2025",
                Description = "Summer 2025",
                Year = 2025,
                StartDate = new DateTime(2024, 8, 6),
                EndDate = new DateTime(2025, 3, 11)
            };

            var assessmentSeries = new List<AssessmentSeries> { summer2022, summer2023, summer2024, summer2025 };
            AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, assessmentSeries);

            return assessmentSeries.ToDictionary(a => a.Name, a => a);
        }

        private OverallResult SeedOverallResult(TqRegistrationPathway registrationPathway, string resultAwarded, string specialismResultAwarded, DateTime createdOn, string details = "")
        {
            var overallResult = new OverallResult
            {
                TqRegistrationPathway = registrationPathway,
                ResultAwarded = resultAwarded,
                SpecialismResultAwarded = specialismResultAwarded,
                CalculationStatus = CalculationStatus.Completed,
                Details = details,
                CreatedOn = createdOn,
                IsOptedin = true
            };

            return OverallResultDataProvider.CreateOverallResult(DbContext, registrationPathway, overallResult);
        }

        private TqPathwayAssessment SeedPathwayAssessment(TqRegistrationPathway registrationPathway, AssessmentSeries assessmentSeries)
        {
            var pathwayAssessment = new TqPathwayAssessment
            {
                TqRegistrationPathway = registrationPathway,
                AssessmentSeries = assessmentSeries,
                IsOptedin = true,
                StartDate = registrationPathway.StartDate
            };

            registrationPathway.TqPathwayAssessments.Add(pathwayAssessment);

            PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
            return pathwayAssessment;
        }

        private TqPathwayResult SeedPathwayResult(TqPathwayAssessment pathwayAssessment, int lookupId, DateTime createdOn, DateTime modifiedOn)
        {
            var pathwayResult = new TqPathwayResult
            {
                TqPathwayAssessment = pathwayAssessment,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true,
                StartDate = pathwayAssessment.StartDate,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn,
                TlLookupId = lookupId
            };

            pathwayAssessment.TqPathwayResults.Add(pathwayResult);

            TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
            return pathwayResult;
        }

        private IndustryPlacement SeedIndustryPlacement(TqRegistrationPathway registrationPathway, DateTime createdOn, DateTime modifiedOn)
        {
            var industryPlacement = new IndustryPlacement
            {
                TqRegistrationPathway = registrationPathway,
                Status = IndustryPlacementStatus.Completed,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn
            };

            registrationPathway.IndustryPlacements.Add(industryPlacement);

            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, industryPlacement);
            return industryPlacement;
        }

        private Dictionary<string, TlSpecialism> SeedSpecialisms(TlPathway pathway)
        {
            var civilEngineering = new TlSpecialism
            {
                LarId = "ZTLOS002",
                Name = "Civil Engineering",
                IsActive = true,
                TlPathway = pathway
            };

            var buildingServicesDesign = new TlSpecialism
            {
                LarId = "ZTLOS003",
                Name = "Building Services Design",
                IsActive = true,
                TlPathway = pathway
            };

            TlSpecialism[] specialisms = new[] { civilEngineering, buildingServicesDesign };

            DbContext.TlSpecialism.AddRange(specialisms);
            return specialisms.ToDictionary(s => s.Name, s => s);
        }

        private TqRegistrationSpecialism SeedRegistrationSpecialism(TqRegistrationPathway registrationPathway, TlSpecialism specialism)
        {
            var registrationSpecialism = new TqRegistrationSpecialism
            {
                TqRegistrationPathway = registrationPathway,
                TlSpecialism = specialism,
                IsOptedin = true,
                StartDate = registrationPathway.StartDate
            };

            return RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, registrationSpecialism);
        }

        private TqSpecialismAssessment SeedSpecialismAssessment(TqRegistrationSpecialism registrationSpecialism, AssessmentSeries assessmentSeries, bool isOptedin = true)
        {
            var specialismAssessment = new TqSpecialismAssessment
            {
                TqRegistrationSpecialism = registrationSpecialism,
                AssessmentSeries = assessmentSeries,
                IsOptedin = isOptedin,
                StartDate = registrationSpecialism.StartDate
            };

            return SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
        }

        private TqSpecialismResult SeedSpecialismResult(TqSpecialismAssessment specialismAssessment, int lookupId, DateTime createdOn, DateTime modifiedOn)
        {
            var specialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessment = specialismAssessment,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true,
                StartDate = specialismAssessment.StartDate,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn,
                TlLookupId = lookupId
            };

            return TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
        }

        private void SeedOverallGradeLookups(TlPathway pathway)
        {
            var lookups = new OverallGradeLookup[]
            {
                new() {
                    TlPathway = pathway,
                    TlLookupCoreGradeId = PathwayComponentGradeAPlus,
                    TlLookupSpecialismGradeId = SpecialismComponentGradeDistinction,
                    TlLookupOverallGradeId = OverallResultGradeDistinctionPlus
                }
            };

            DbContext.OverallGradeLookup.AddRange(lookups);
        }

        #endregion
    }
}
