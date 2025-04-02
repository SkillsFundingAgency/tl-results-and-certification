using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.OverallResultCalculationRepositoryBaseTests.GetLearnersForOverallGradeCalculationTests
{
    public abstract class GetLearnersForOverallGradeCalculationBaseTest : BaseTest<TqRegistrationPathway>
    {
        protected Dictionary<string, AssessmentSeries> CoreAssessmentSeries;
        protected List<TqRegistrationPathway> ExpectedResults = new();

        protected IOverallResultCalculationRepository CreateOverallResultCalculationRepository()
        {
            var config = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    NoOfAcademicYearsToProcess = 2
                }
            };

            return new OverallResultCalculationRepository(DbContext, config);
        }

        protected void SeedTestData()
        {
            CoreAssessmentSeries = SeedCoreAssessmentSeries();
            Dictionary<string, AssessmentSeries> specialismAssessmentSeries = SeedSpecialismAssessmentSeries();

            TqAwardingOrganisation pearson = SeedAwardingOrganisation(EnumAwardingOrganisation.Pearson);
            TqAwardingOrganisation ncfe = SeedAwardingOrganisation(EnumAwardingOrganisation.Ncfe);

            TqProvider barnsleyCollege = SeedProvider(pearson, 10000536, "Barnsley College");
            TqProvider derbyCollege = SeedProvider(pearson, 10001919, "Derby College");
            TqProvider bathCollege = SeedProvider(ncfe, 10001465, "Bath College");

            Dictionary<string, TlSpecialism> specialisms = SeedSpecialisms(pearson.TlPathway);

            // Registration is active and between bounds (it should be returned).
            TqRegistrationProfile johnSmith = SeedRegistrationProfile(1111111111, "John", "Smith", new DateTime(2000, 1, 1));
            TqRegistrationPathway johnSmithRegistration = SeedRegistrationPathway(johnSmith, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            ExpectedResults.Add(johnSmithRegistration);

            // Registration is withdrawn (it should NOT be returned).
            TqRegistrationProfile jessicaJohnson = SeedRegistrationProfile(1111111112, "Jessica", "Johnson", new DateTime(2002, 5, 17));
            SeedRegistrationPathway(jessicaJohnson, derbyCollege, 2022, RegistrationPathwayStatus.Withdrawn);

            // Registration is active and academic year outside bounds (it should NOT be returned).
            TqRegistrationProfile peterSmith = SeedRegistrationProfile(1111111113, "Peter", "Smith", new DateTime(1999, 8, 6));
            SeedRegistrationPathway(peterSmith, bathCollege, 2020, RegistrationPathwayStatus.Active);

            // Registration is active and has an overall result (it should NOT be returned).
            TqRegistrationProfile oliverBennet = SeedRegistrationProfile(1111111114, "Oliver", "Bennett", new DateTime(2005, 4, 16));
            TqRegistrationPathway oliverBennetRegistration = SeedRegistrationPathway(oliverBennet, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(oliverBennetRegistration, new DateTime(2022, 1, 1));

            // Registration is active, has an overall result and an industry placement created afterwards (it should be returned).
            TqRegistrationProfile jessicaRoberts = SeedRegistrationProfile(1111111120, "Jessica", "Roberts", new DateTime(2004, 9, 5));
            TqRegistrationPathway jessicaRobertsRegistration = SeedRegistrationPathway(jessicaRoberts, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(jessicaRobertsRegistration, new DateTime(2024, 10, 15));
            SeedIndustryPlacement(jessicaRobertsRegistration, new DateTime(2024, 10, 16), new DateTime(2024, 10, 16));
            ExpectedResults.Add(jessicaRobertsRegistration);

            // Registration is active, has an overall result and an industry placement modified afterwards (it should be returned).
            TqRegistrationProfile lucyWalker = SeedRegistrationProfile(1111111121, "Lucy", "Walker", new DateTime(2005, 8, 25));
            TqRegistrationPathway lucyWalkerRegistration = SeedRegistrationPathway(lucyWalker, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(lucyWalkerRegistration, new DateTime(2024, 6, 1));
            SeedIndustryPlacement(lucyWalkerRegistration, new DateTime(2024, 4, 1), new DateTime(2024, 6, 1, 10, 0, 0));
            ExpectedResults.Add(lucyWalkerRegistration);

            // Registration is active, has an overall result and an industry placement created before (it should NOT be returned).
            TqRegistrationProfile graceBaker = SeedRegistrationProfile(1111111122, "Grace", "Baker", new DateTime(2000, 12, 31));
            TqRegistrationPathway graceBakerRegistration = SeedRegistrationPathway(graceBaker, bathCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(graceBakerRegistration, new DateTime(2024, 12, 10));
            SeedIndustryPlacement(graceBakerRegistration, new DateTime(2024, 12, 9), new DateTime(2024, 12, 9));

            // Registration is active, has an overall result and an industry placement modified before (it should NOT be returned).
            TqRegistrationProfile miaTurner = SeedRegistrationProfile(1111111123, "Mia", "Turner", new DateTime(2001, 11, 3));
            TqRegistrationPathway miaTurnerRegistration = SeedRegistrationPathway(miaTurner, bathCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(miaTurnerRegistration, new DateTime(2023, 4, 4));
            SeedIndustryPlacement(miaTurnerRegistration, new DateTime(2023, 1, 1), new DateTime(2023, 4, 3));

            // Registration is active, has an overall result and a core result created afterwards (it should be returned).
            TqRegistrationProfile charlotteDavies = SeedRegistrationProfile(1111111115, "Charlotte", "Davies", new DateTime(2004, 2, 25));
            TqRegistrationPathway charlotteDaviesRegistration = SeedRegistrationPathway(charlotteDavies, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(charlotteDaviesRegistration, new DateTime(2022, 1, 1));
            TqPathwayAssessment charlotteDaviesPathwayAssessment = SeedPathwayAssessment(charlotteDaviesRegistration, CoreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(charlotteDaviesPathwayAssessment, new DateTime(2022, 1, 2), new DateTime(2022, 1, 2));
            ExpectedResults.Add(charlotteDaviesRegistration);

            // Registration is active, has an overall result and a core result modified afterwards (it should be returned).
            TqRegistrationProfile emilyClarke = SeedRegistrationProfile(1111111116, "Emily", "Clarke", new DateTime(2003, 11, 2));
            TqRegistrationPathway emilyClarkeRegistration = SeedRegistrationPathway(emilyClarke, derbyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(emilyClarkeRegistration, new DateTime(2023, 10, 12));
            TqPathwayAssessment emilyClarkePathwayAssessment = SeedPathwayAssessment(emilyClarkeRegistration, CoreAssessmentSeries["Summer 2024"]);
            SeedPathwayResult(emilyClarkePathwayAssessment, new DateTime(2023, 9, 2), new DateTime(2023, 10, 13));
            ExpectedResults.Add(emilyClarkeRegistration);

            // Registration is active, has an overall result and a core result created before (it should NOT be returned).
            TqRegistrationProfile jamesThompson = SeedRegistrationProfile(1111111117, "James", "Thompson", new DateTime(2001, 8, 2));
            TqRegistrationPathway jamesThompsonRegistration = SeedRegistrationPathway(jamesThompson, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(jamesThompsonRegistration, new DateTime(2022, 1, 2));
            TqPathwayAssessment jamesThompsonPathwayAssessment = SeedPathwayAssessment(jamesThompsonRegistration, CoreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(jamesThompsonPathwayAssessment, new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));

            // Registration is active, has an overall result and a core result modified before (it should NOT be returned).
            TqRegistrationProfile freddieWhitmore = SeedRegistrationProfile(1111111118, "Freddie", "Whitmore", new DateTime(2002, 12, 12));
            TqRegistrationPathway freddieWhitmoreRegistration = SeedRegistrationPathway(freddieWhitmore, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(freddieWhitmoreRegistration, new DateTime(2024, 10, 20));
            TqPathwayAssessment freddieWhitmorePathwayAssessment = SeedPathwayAssessment(freddieWhitmoreRegistration, CoreAssessmentSeries["Autumn 2024"]);
            SeedPathwayResult(freddieWhitmorePathwayAssessment, new DateTime(2024, 10, 10), new DateTime(2024, 10, 19));

            // Registration is active and a core result of a future assessment series (it should NOT be returned).
            TqRegistrationProfile williamBrown = SeedRegistrationProfile(1111111119, "William", "Brown", new DateTime(2004, 2, 25));
            TqRegistrationPathway williamBrownRegistration = SeedRegistrationPathway(williamBrown, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(williamBrownRegistration, new DateTime(2022, 1, 1));
            TqPathwayAssessment williamBrownPathwayAssessment = SeedPathwayAssessment(williamBrownRegistration, CoreAssessmentSeries["Summer 2025"]);
            SeedPathwayResult(williamBrownPathwayAssessment, new DateTime(2022, 1, 20), new DateTime(2022, 1, 20));

            // Registration is active, has an overall result and a specialism result created afterwards (it should be returned).
            TqRegistrationProfile henryLewis = SeedRegistrationProfile(1111111120, "Henry", "Lewis", new DateTime(2001, 3, 3));
            TqRegistrationPathway henryLewisRegistration = SeedRegistrationPathway(henryLewis, barnsleyCollege, 2022, RegistrationPathwayStatus.Active);
            SeedOverallResult(henryLewisRegistration, new DateTime(2022, 1, 1));
            TqRegistrationSpecialism henryLewisRegistrationSpecialism = SeedRegistrationSpecialism(henryLewisRegistration, specialisms["Civil Engineering"]);
            TqSpecialismAssessment henryLewisSpecialismAssessment = SeedSpecialismAssessment(henryLewisRegistrationSpecialism, specialismAssessmentSeries["Summer 2024"]);
            SeedSpecialismResult(henryLewisSpecialismAssessment, new DateTime(2022, 1, 2), new DateTime(2022, 1, 13));
            ExpectedResults.Add(henryLewisRegistration);

            DbContext.SaveChanges();
        }

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
                ResultCalculationYear = 2020
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
                ResultCalculationYear = 2020
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
                ResultCalculationYear = 2021
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
                ResultCalculationYear = 2021
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
                ResultCalculationYear = 2022
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
                ResultCalculationYear = 2022
            };

            var summer2025 = new AssessmentSeries
            {
                Id = 12,
                ComponentType = ComponentType.Core,
                Name = "Summer 2025",
                Description = "Summer 2025",
                Year = 2025,
                StartDate = new DateTime(2024, 8, 6),
                EndDate = new DateTime(2025, 3, 10),
                ResultCalculationYear = 2023
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

        private OverallResult SeedOverallResult(TqRegistrationPathway registrationPathway, DateTime createdOn)
        {
            var overallResult = new OverallResult
            {
                TqRegistrationPathway = registrationPathway,
                ResultAwarded = "Distinction",
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

        private TqPathwayResult SeedPathwayResult(TqPathwayAssessment pathwayAssessment, DateTime createdOn, DateTime modifiedOn)
        {
            var pathwayResult = new TqPathwayResult
            {
                TqPathwayAssessment = pathwayAssessment,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true,
                StartDate = pathwayAssessment.StartDate,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn,
                TlLookup = new TlLookup
                {
                    IsActive = true,
                    Category = "PathwayComponentGrade",
                    Code = "PCG1",
                    Value = "A*"
                }
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

        private TqSpecialismAssessment SeedSpecialismAssessment(TqRegistrationSpecialism registrationSpecialism, AssessmentSeries assessmentSeries)
        {
            var specialismAssessment = new TqSpecialismAssessment
            {
                TqRegistrationSpecialism = registrationSpecialism,
                AssessmentSeries = assessmentSeries,
                IsOptedin = true,
                StartDate = registrationSpecialism.StartDate
            };

            return SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
        }

        private TqSpecialismResult SeedSpecialismResult(TqSpecialismAssessment specialismAssessment, DateTime createdOn, DateTime modifiedOn)
        {
            var specialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessment = specialismAssessment,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true,
                StartDate = specialismAssessment.StartDate,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn,
                TlLookup = new TlLookup
                {
                    IsActive = true,
                    Category = "PathwayComponentGrade",
                    Code = "PCG1",
                    Value = "A*"
                }
            };

            return TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
        }
    }
}