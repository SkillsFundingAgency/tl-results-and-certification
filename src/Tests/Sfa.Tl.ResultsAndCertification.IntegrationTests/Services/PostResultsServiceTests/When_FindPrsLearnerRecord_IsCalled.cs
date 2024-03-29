﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_FindPrsLearnerRecord_IsCalled : PostResultsServiceServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private FindPrsLearnerRecord _actualResult;
        private List<TqPathwayAssessment> _tqPathwayAssessmentsSeedData;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },    // No assessment
                { 1111111112, RegistrationPathwayStatus.Withdrawn }, // Assessment + Result
                { 1111111113, RegistrationPathwayStatus.Active },    // Assessment + Result
                { 1111111114, RegistrationPathwayStatus.Active }     // Multi Assessment 
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            DbContext.SaveChanges();
            TransferRegistration(1111111113, Provider.WalsallCollege);

            // Seed Assessments And Results
            _tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();

            var profilesWithAssessment = new List<long> { 1111111112, 1111111113, 1111111114 };
            var profilesWithResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, null), (1111111114, null) };
            foreach (var profile in _profiles.Where(x => profilesWithAssessment.Contains(x.UniqueLearnerNumber)))
            {
                var isLatestActive = _ulns[profile.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(profile.TqRegistrationPathways.ToList(), isLatestActive);
                _tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Seed Pathway results
                foreach (var assessment in pathwayAssessments.Where(x => profilesWithResults.Any(p => p.Item1 == x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)))
                {
                    var hasHitoricData = new List<long> { 1111111113 };
                    var hasHistoricResult = hasHitoricData.Any(x => x == profile.UniqueLearnerNumber);
                    var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                    var seedPathwayResultsAsActive = assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber != 1111111112;
                    GetPathwayResultDataToProcess(assessment, seedPathwayResultsAsActive, hasHistoricResult, prsStatus);
                }
            }

            // Additional assessment to 1111111114
            var prof = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114);
            var asmnt = GetPathwayAssessmentsDataToProcess(prof.TqRegistrationPathways.ToList(), true);
            _tqPathwayAssessmentsSeedData.AddRange(asmnt);

            // Seed Assessments
            SeedPathwayAssessmentsData(_tqPathwayAssessmentsSeedData, true);

            // Test class and dependencies
            CreateMapper();
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
            var pathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultsRepository = new GenericRepository<TqPathwayResult>(pathwayResultRepositoryLogger, DbContext);

            var specialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultsRepository = new GenericRepository<TqSpecialismResult>(specialismResultRepositoryLogger, DbContext);

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);

            Configuration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };

            PostResultsServiceServiceLogger = new Logger<PostResultsServiceService>(new NullLoggerFactory());
            PostResultsServiceService = new PostResultsServiceService(Configuration, PostResultsServiceRepository, PathwayResultsRepository, SpecialismResultsRepository, NotificationService, PostResultsServiceMapper, PostResultsServiceServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, long? uln, int? profileId, bool callWithUln)
        {
            if (_actualResult != null)
                return;

            if (callWithUln)
                _actualResult = await PostResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln);
            else
                _actualResult = await PostResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, null, profileId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, AwardingOrganisation ao, bool callWithUln, bool isRecordFound)
        {
            var expectedProfile = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);

            // When
            await WhenAsync((long)ao, uln, expectedProfile?.Id, callWithUln);

            if (isRecordFound == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

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

            var expectedAssessments = _profiles.Where(x => x.UniqueLearnerNumber == uln)
                .SelectMany(x => x.TqRegistrationPathways.Last().TqPathwayAssessments.Where(x => x.EndDate == null && x.IsOptedin))
                .OrderBy(o => o.Id).ToList();

            var actualAssessments = _actualResult.PathwayAssessments.OrderBy(o => o.AssessmentId);
            actualAssessments.Count().Should().Be(expectedAssessments.Count());

            for (int i = 0; i < expectedAssessments.Count(); i++)
            {
                var hasResult = expectedAssessments[i].TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null);
                actualAssessments.ElementAt(i).AssessmentId.Should().Be(expectedAssessments[i].Id);
                actualAssessments.ElementAt(i).SeriesName.Should().Be(expectedAssessments[i].AssessmentSeries.Name);
                actualAssessments.ElementAt(i).HasResult.Should().Be(hasResult);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                bool callWithUln = true;
                bool recoundFound = true;
                return new[]
                {
                    // Call with Uln
                    new object[] { 9999999999, AwardingOrganisation.Pearson, callWithUln, !recoundFound }, // Invalid Uln
                    new object[] { 1111111111, AwardingOrganisation.Pearson, callWithUln, recoundFound }, // Active + No Assessments
                    new object[] { 1111111111, AwardingOrganisation.Ncfe, callWithUln, !recoundFound },
                    new object[] { 1111111112, AwardingOrganisation.Pearson, callWithUln, recoundFound }, // Withdrawn
                    new object[] { 1111111113, AwardingOrganisation.Pearson, callWithUln, recoundFound }, // Active + Single Assessment
                    new object[] { 1111111114, AwardingOrganisation.Pearson, callWithUln, recoundFound }, // Active + Multiple Assessments

                    // Call with ProfileId
                    new object[] { 9999999999, AwardingOrganisation.Pearson, !callWithUln, !recoundFound }, // Invalid Uln
                    new object[] { 1111111111, AwardingOrganisation.Pearson, !callWithUln, recoundFound }, // Active + No Assessments
                    new object[] { 1111111111, AwardingOrganisation.Ncfe, !callWithUln, !recoundFound },
                    new object[] { 1111111112, AwardingOrganisation.Pearson, !callWithUln, recoundFound }, // Withdrawn
                    new object[] { 1111111113, AwardingOrganisation.Pearson, !callWithUln, recoundFound }, // Active + Single Assessment
                    new object[] { 1111111114, AwardingOrganisation.Pearson, !callWithUln, recoundFound }, // Active + Multiple Assessments
                };
            }
        }

        private void TransferRegistration(long uln, Provider transferTo)
        {
            var toProvider = DbContext.TlProvider.FirstOrDefault(x => x.UkPrn == (long)transferTo);
            var transferToTqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, toProvider, true);

            var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);

            foreach (var pathway in profile.TqRegistrationPathways)
            {
                pathway.Status = RegistrationPathwayStatus.Transferred;
                pathway.EndDate = DateTime.UtcNow;

                foreach (var specialism in pathway.TqRegistrationSpecialisms)
                {
                    specialism.IsOptedin = true;
                    specialism.EndDate = DateTime.UtcNow;
                }
            }

            var industryPlacement = profile.TqRegistrationPathways.FirstOrDefault()?.IndustryPlacements?.FirstOrDefault();
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, profile, transferToTqProvider);
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (industryPlacement != null)
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, tqRegistrationPathway.Id, industryPlacement.Status);

            DbContext.SaveChanges();
        }
    }
}