﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_WithdrawRegistration_Called : RegistrationServiceBaseTest
    {
        private bool _result;
        private WithdrawRegistrationRequest _withdrawRegistrationRequest;
        private long _uln;
        private TqRegistrationPathway _tqRegistrationPathway;
        private TqRegistrationProfile _tqRegistrationProfile;
        private List<OverallGradeLookup> _overallGradeLookup;

        public override void Given()
        {
            // Seed Tlevel data for pearson
            _uln = 1111111111;
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _tqRegistrationProfile = SeedRegistrationData(_uln);

            // Pathway Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = _tqRegistrationProfile.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(seededRegistrationPathways, isBulkUpload: false));
            var pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Pathway results seed
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var assessment in pathwayAssessments)
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isBulkUpload: false));

            var pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData);

            // Specialisms assessment seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.SelectMany(x => x.TqRegistrationSpecialisms).ToList()));
            var specialismsAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            // Specialism results seed
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();
            foreach (var assessment in specialismsAssessments)
                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }, isBulkUpload: false));

            var specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData);

            // Seed OverallResultLookup
            _overallGradeLookup = new List<OverallGradeLookup>();
            var pathwayId = seededRegistrationPathways.FirstOrDefault().Id;
            var coreResultId = pathwayResults.FirstOrDefault().TlLookupId;
            var splResultId = specialismResults.FirstOrDefault().TlLookupId;
            _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 1, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 1 });
            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

            // Seed Overall results
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = pathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, CertificateType = PrintCertificateType.Certificate, StartDate = DateTime.UtcNow.AddMonths(-1), IsOptedin = true, EndDate = DateTime.UtcNow, CreatedOn = DateTime.UtcNow } }, true);


            CreateMapper();

            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqPathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(TqPathwayAssessmentRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqPathwayAssessmentRepository, TqRegistrationSpecialismRepository, CommonService, SystemProvider, RegistrationMapper, RegistrationRepositoryLogger);

            _withdrawRegistrationRequest = new WithdrawRegistrationRequest
            {
                AoUkprn = TlAwardingOrganisation.UkPrn,
                PerformedBy = "Test User"
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await RegistrationService.WithdrawRegistrationAsync(_withdrawRegistrationRequest);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(int profileId, bool expectedResult)
        {
            _withdrawRegistrationRequest.ProfileId = profileId;
            await WhenAsync();
            _result.Should().Be(expectedResult);

            if (expectedResult)
            {
                var expectedRegistrationProfile = _tqRegistrationProfile;
                var actualRegistrationProfile = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _tqRegistrationProfile.UniqueLearnerNumber)
                                                                                                                           .Include(x => x.TqRegistrationPathways)
                                                                                                                               .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                                                    .ThenInclude(x => x.TqSpecialismAssessments)
                                                                                                                                        .ThenInclude(x => x.TqSpecialismResults)
                                                                                                                           .Include(x => x.TqRegistrationPathways)
                                                                                                                                .ThenInclude(x => x.TqPathwayAssessments)
                                                                                                                                    .ThenInclude(x => x.TqPathwayResults)
                                                                                                                           .Include(x => x.TqRegistrationPathways)
                                                                                                                                .ThenInclude(x => x.OverallResults)
                                                                                                                           .FirstOrDefault();

                // assert registration profile data
                actualRegistrationProfile.Should().NotBeNull();
                actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
                actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
                actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
                actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
                actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

                // Assert registration pathway data
                actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Active).ToList().Count.Should().Be(0);
                actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Withdrawn).ToList().Count.Should().Be(1);

                // Assert Any Active Pathway
                actualRegistrationProfile.TqRegistrationPathways.Any(x => x.Status == RegistrationPathwayStatus.Active).Should().BeFalse();

                // Assert Withdrawn Pathway
                var actualWithdrawnPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => expectedRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
                var expectedWithdrawnPathway = expectedRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
                AssertRegistrationPathway(actualWithdrawnPathway, expectedWithdrawnPathway);

                // Assert withdrawn PathwayAssessments should not have any active assessments
                actualWithdrawnPathway.TqPathwayAssessments.Any(x => x.EndDate == null).Should().BeFalse();

                // Assert pending withdrawal is false
                actualWithdrawnPathway.IsPendingWithdrawal.Should().BeFalse();

                // Assert Withdrawn PathwayAssessment
                var actualWithdrawnAssessment = actualWithdrawnPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
                var expectedWithdrawnAssessment = expectedWithdrawnPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
                AssertPathwayAssessment(actualWithdrawnAssessment, expectedWithdrawnAssessment);

                // Assert withdrawn record should not have active SpecialismAssessments
                actualWithdrawnPathway.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Any(x => x.EndDate == null).Should().BeFalse();

                // Assert Withdrawn SpecialismAssessment
                var actualWithdrawnSpecialismAssessments = actualWithdrawnPathway.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Where(x => x.EndDate != null);
                var expectedWithdrawnSpecialismAssessments = expectedWithdrawnPathway.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Where(x => x.EndDate != null);
                AssertSpecialismAssessment(actualWithdrawnSpecialismAssessments, expectedWithdrawnSpecialismAssessments);

                // Assert withdrawn record should not have active PathwayResult
                foreach (var pathwayAssessment in actualWithdrawnPathway.TqPathwayAssessments)
                {
                    pathwayAssessment.TqPathwayResults.Any(x => x.EndDate == null).Should().BeFalse();
                }

                // Assert Withdrawn PathwayResult
                var actualWithdrawnResult = actualWithdrawnAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
                var expectedWithdrawndResult = expectedWithdrawnAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
                AssertPathwayResults(actualWithdrawnResult, expectedWithdrawndResult);

                // Assert withdrawn record should not have active SpecialismResult
                foreach (var actualWithdrawnSpecialismAssessment in actualWithdrawnSpecialismAssessments)
                {
                    actualWithdrawnSpecialismAssessment.TqSpecialismResults.Any(x => x.EndDate == null).Should().BeFalse();
                }

                // Assert Withdrawn Specialism Assessment Result
                foreach (var actualWithdrawnSpecialismAssessment in actualWithdrawnSpecialismAssessments)
                {
                    var expectedWithdrawnSpecialismResult = expectedWithdrawnSpecialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == actualWithdrawnSpecialismAssessment.TqRegistrationSpecialismId);

                    var actualWithdrawnSpecialismResult = actualWithdrawnSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.EndDate != null);
                    var expectedWithdrawndSpecialismResult = expectedWithdrawnSpecialismResult.TqSpecialismResults.FirstOrDefault(x => x.EndDate != null);

                    AssertSpecialismResult(actualWithdrawnSpecialismResult, expectedWithdrawndSpecialismResult);
                }

                // Assert Active Overall result
                var actualWithdrawnOverallResult = actualWithdrawnPathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate != null);
                var expectedWithdrawnOverallResult = expectedWithdrawnPathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate != null);
                AssertOverallResult(actualWithdrawnOverallResult, expectedWithdrawnOverallResult, true);
            }
        }

        public static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment, bool isRejoin = false)
        {
            actualAssessment.Should().NotBeNull();
            if (!isRejoin)
                actualAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);

            actualAssessment.TqRegistrationPathway.TqProviderId.Should().Be(expectedAssessment.TqRegistrationPathway.TqProviderId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();

            if (actualAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1, true },
                    new object[] { 10000000, false }
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);

            TqProviders = new List<TqProvider>();
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();

            foreach (var tlProvider in TlProviders)
            {
                TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider));
            }

            DbContext.SaveChangesAsync();
        }

        private TqRegistrationProfile SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            _tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProviders.First());

            foreach (var specialism in Specialisms)
            {
                _tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, _tqRegistrationPathway, specialism));
            }

            DbContext.SaveChangesAsync();
            return profile;
        }
    }
}
