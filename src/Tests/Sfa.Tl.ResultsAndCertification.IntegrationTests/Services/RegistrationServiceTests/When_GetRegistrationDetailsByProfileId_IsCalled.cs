using FluentAssertions;
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
    public class When_GetRegistrationDetailsByProfileId_IsCalled : RegistrationServiceBaseTest
    {
        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            var pearsonRegistrationProfile = SeedRegistrationData(1111111111, RegistrationPathwayStatus.Withdrawn);
            var profile = SeedRegistrationData(1111111112);

            // Seed specialism assesment entries for Uln 1111111112
            SeedSpecialismAssessmentsData(GetSpecialismAssessmentsDataToProcess(profile.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList()));

            // Uln 1111111111 is withdrawn from Pearson above registerign with NCFE below. 
            SeedTestData(EnumAwardingOrganisation.Ncfe, true);
            var ncfeRegistrationProfile = SeedRegistrationData(1111111111, RegistrationPathwayStatus.Active, false);
            ncfeRegistrationProfile.TqRegistrationPathways.ToList().ForEach(x => { x.TqRegistrationProfileId = pearsonRegistrationProfile.Id; });
            pearsonRegistrationProfile.TqRegistrationPathways.Add(ncfeRegistrationProfile.TqRegistrationPathways.ToList()[0]);
            
            DbContext.SaveChanges();

            CreateMapper();

            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);
        }

        public override Task When() 
        {
            return Task.CompletedTask;
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long aoUkprn, int profileId, RegistrationDetails expectedResponse)
        {
            var actualResult = RegistrationService.GetRegistrationDetailsAsync(aoUkprn, profileId).Result;

            if (actualResult == null)
            {
                expectedResponse.Should().BeNull();
                return;
            }

            actualResult.Uln.Should().Be(expectedResponse.Uln);
            actualResult.Firstname.Should().Be(expectedResponse.Firstname);
            actualResult.Lastname.Should().Be(expectedResponse.Lastname);
            actualResult.DateofBirth.Should().Be(expectedResponse.DateofBirth);
            actualResult.ProviderName.Should().Be(expectedResponse.ProviderName);
            actualResult.PathwayName.Should().Be(expectedResponse.PathwayName);

            actualResult.Specialisms.Should().NotBeNullOrEmpty();
            actualResult.Specialisms.Count().Should().Be(expectedResponse.Specialisms?.Count());

            actualResult.Specialisms.ToList().ForEach(x => 
            { 
                expectedResponse.Specialisms?.Select(s => s.Code).Should().Contain(x.Code);
                expectedResponse.Specialisms?.Select(s => s.Name).Should().Contain(x.Name);
            });

            actualResult.AcademicYear.Should().Be(expectedResponse.AcademicYear);
            actualResult.Status.Should().Be(expectedResponse.Status);
            actualResult.IsActiveWithOtherAo.Should().Be(expectedResponse.IsActiveWithOtherAo);
            actualResult.HasActiveAssessmentEntriesForSpecialisms.Should().Be(expectedResponse.HasActiveAssessmentEntriesForSpecialisms);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == 1111111111);
                var tlProvider = new TlProviderBuilder().Build();
                var tlPathway = new TlPathwayBuilder().Build(EnumAwardingOrganisation.Pearson, new TlRouteBuilder().Build(EnumAwardingOrganisation.Pearson));
                var tlSpecialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, tlPathway);
                var tqRegistrationPathway = new TqRegistrationPathwayBuilder().Build();

                var activeProfile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == 1111111112);
                return new[]
                {
                     // Uln not found
                    new object[] { 10011881, 100, null },
                    
                    // IsActiveWithOtherAo i.e NCFE. 
                    new object[] { 10011881, 1,
                        new RegistrationDetails
                        {
                            Uln = profile.UniqueLearnerNumber,
                            Firstname = profile.Firstname,
                            Lastname = profile.Lastname,
                            DateofBirth = profile.DateofBirth,
                            AoUkprn = 10011881,
                            ProfileId = 1,
                            PathwayLarId = tlPathway.LarId,
                            ProviderUkprn = tlProvider.UkPrn,
                            ProviderName = tlProvider.Name,
                            PathwayName = tlPathway.Name,
                            Specialisms = tlSpecialisms.Select(x => new SpecialismDetails { Name = x.Name , Code = x.LarId}),
                            AcademicYear = tqRegistrationPathway.AcademicYear,
                            Status = RegistrationPathwayStatus.Withdrawn,
                            IsActiveWithOtherAo = true,
                            HasActiveAssessmentEntriesForSpecialisms = false
                        }
                    },
                    // IsActive
                    new object[] { 10011881, 2,
                        new RegistrationDetails
                        {
                            Uln = activeProfile.UniqueLearnerNumber,
                            Firstname = activeProfile.Firstname,
                            Lastname = activeProfile.Lastname,
                            DateofBirth = activeProfile.DateofBirth,
                            AoUkprn = 10011881,
                            ProfileId = 2,
                            PathwayLarId = tlPathway.LarId,
                            ProviderUkprn = tlProvider.UkPrn,
                            ProviderName = tlProvider.Name,
                            PathwayName = tlPathway.Name,
                            Specialisms = tlSpecialisms.Select(x => new SpecialismDetails { Name = x.Name , Code = x.LarId}),
                            AcademicYear = tqRegistrationPathway.AcademicYear,
                            Status = RegistrationPathwayStatus.Active,
                            IsActiveWithOtherAo = false,
                            HasActiveAssessmentEntriesForSpecialisms = true
                        }
                    }
                };
            }
        }

        private TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, bool saveChange = true)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProvider);
            tqRegistrationPathway.Status = status;

            
            foreach (var specialism in Specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
            }

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                foreach (var specialism in tqRegistrationPathway.TqRegistrationSpecialisms)
                {
                    specialism.EndDate = DateTime.UtcNow.AddDays(-1);
                }
            }


            if (saveChange)
                DbContext.SaveChangesAsync();
            
            return tqRegistrationProfile;
        }
    }
}
