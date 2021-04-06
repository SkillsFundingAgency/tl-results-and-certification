using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_UpdateLearnerRecordAsync_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private bool _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement)>
            {
                (1111111111, false, true, true, true, true), // Lrs data and Learner Record added already
                (1111111112, false, true, false, false, false), // Lrs data and Learner Record not added
                (1111111113, true, false, false, false, false), // Not from Lrs and Learner Record not added
                (1111111114, true, false, false, true, true), // Not from Lrs and Learner Record added
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement);
            }

            DbContext.SaveChanges();

            CreateMapper();

            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            IndustryPlacementRepositoryLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementRepositoryLogger, DbContext);

            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, RegistrationPathwayRepository, IndustryPlacementRepository, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(UpdateLearnerRecordRequest request)
        {
            _actualResult = await TrainingProviderService.UpdateLearnerRecordAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(UpdateLearnerRecordRequest request, bool expectedResult)
        {
            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == request.Uln);
            expectedProfile.Should().NotBeNull();
            
            var expectedPathway = expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn);
            expectedPathway.Should().NotBeNull();

            var expectedIndustryPlacement = expectedPathway.IndustryPlacements.FirstOrDefault();

            request.ProfileId = expectedProfile.Id;
            request.RegistrationPathwayId = expectedPathway.Id;
            request.IndustryPlacementId = expectedIndustryPlacement?.Id ?? 0;

            await WhenAsync(request);

            if (expectedResult == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            _actualResult.Should().BeTrue();

            var actualPathway = DbContext.TqRegistrationPathway.Where(p => p.TqRegistrationProfile.UniqueLearnerNumber == expectedProfile.UniqueLearnerNumber && p.TqProvider.TlProvider.UkPrn == request.Ukprn)
                                                                .Include(p => p.IndustryPlacements)
                                                                .Include(p => p.TqRegistrationProfile)
                                                                .OrderByDescending(p => p.CreatedOn).FirstOrDefault();

            actualPathway.Should().NotBeNull();

            // Assert Profile data

            actualPathway.TqRegistrationProfile.Id.Should().Be(expectedProfile.Id);
            actualPathway.TqRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);

            // Assert IndustryPlacement data
            actualPathway.IndustryPlacements.Count().Should().Be(1);

            var actualIndustryPlacement = actualPathway.IndustryPlacements.Single();
            actualIndustryPlacement.TqRegistrationPathwayId.Should().Be(actualPathway.Id);
            actualIndustryPlacement.Status.Should().Be(request.IndustryPlacementStatus);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Invalid Provider Ukprn - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = 0000000000, Uln = 1111111111, HasIndustryPlacementChanged = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" }, false },

                    // Learner Record Added but HasIndustryPlacementChanged set to False - returs false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111111, HasIndustryPlacementChanged = false, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" }, false },

                    // Learner Record not Added (LRS data with no industry placement) - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111112, HasIndustryPlacementChanged = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" }, false },

                    // Not from Lrs and Learner Record added already - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111113, HasIndustryPlacementChanged = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" }, false },

                    // Not from Lrs and Learner Record not added - return true
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111114, HasIndustryPlacementChanged = true, IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration, PerformedBy = "Test User" }, true }
                };
            }
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TrainingProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<UpdateLearnerRecordRequest, IndustryPlacement>(new DateTimeProvider()) :
                                null);
            });
            TrainingProviderMapper = new Mapper(mapperConfig);
        }
    }
}