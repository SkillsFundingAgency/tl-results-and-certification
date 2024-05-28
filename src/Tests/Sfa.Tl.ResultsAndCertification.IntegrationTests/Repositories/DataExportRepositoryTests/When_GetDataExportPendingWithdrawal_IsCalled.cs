using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.DataExportRepositoryTests
{
    public class When_GetDataExportPendingWithdrawal_IsCalled : DataExportRepositoryBaseTest
    {
        private List<TqRegistrationProfile> _registrations;
        private IList<PendingWithdrawalsExport> _actualResult;

        public override void Given()
        {
            SeedData();
            DataExportRepository = new DataExportRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn)
        {
            if (_actualResult != null)
                return;

            _actualResult = await DataExportRepository.GetDataExportPendingWithdrawalsAsync(aoUkprn);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, List<PendingWithdrawalsExport> exports)
        {
            await WhenAsync(aoUkprn);
            _actualResult.Should().BeEquivalentTo(exports);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        AwardingOrganisation.Pearson,
                        new List<PendingWithdrawalsExport>()
                    },
                    new object[]
                    {
                        AwardingOrganisation.CityAndGuilds,
                        new List<PendingWithdrawalsExport>
                        {
                            new()
                            {
                                Uln = 1111111114,
                                FirstName = "Sophie",
                                LastName = "Williams",
                                DateOfBirth = new DateTime(2005, 10, 25),
                                Ukprn = 10000536,
                                AcademicYear = 2023,
                                Core = "60358300",
                                SpecialismsList = new List<string>{ "ZTLOS001" },
                                CreatedOn = new DateTime(2024, 6, 1)
                            }
                        }
                    },
                    new object[]
                    {
                        AwardingOrganisation.Ncfe,
                        new List<PendingWithdrawalsExport>
                        {
                            new()
                            {
                                Uln = 1111111111,
                                FirstName = "James",
                                LastName = "Smith",
                                DateOfBirth = new DateTime(2000, 1, 1),
                                Ukprn = 10000536,
                                AcademicYear = 2020,
                                Core = "60358300",
                                SpecialismsList = new List<string>{ "ZTLOS001" },
                                CreatedOn = new DateTime(2021, 1, 1)
                            }
                        }
                    }
                };
            }
        }

        private void SeedData()
        {
            TlProvider barnsleyCollegeProvider = new()
            {
                IsActive = true,
                UkPrn = 10000536,
                Name = "Barnsley College",
                DisplayName = "Barnsley College"
            };

            TlAwardingOrganisation nfce = new()
            {
                IsActive = true,
                UkPrn = (long)AwardingOrganisation.Ncfe,
                Name = "Ncfe",
                DisplayName = "Ncfe",
            };

            TlAwardingOrganisation cityAndGuilds = new()
            {
                IsActive = true,
                UkPrn = (long)AwardingOrganisation.CityAndGuilds,
                Name = "City & Guilds",
                DisplayName = "City & Guilds",
            };

            TlPathway designSurveyingPathway = new()
            {
                IsActive = true,
                LarId = "60358300",
                TlevelTitle = "T Level in Design, Surveying and Planning for Construction",
                Name = "Design, Surveying and Planning"
            };

            TlSpecialism surveyingSpecialism = new()
            {
                LarId = "ZTLOS001",
                Name = "Surveying and Design for Construction and the Built Environment",
                IsActive = true
            };

            TqRegistrationPathway firstRegPathway = new()
            {
                TqRegistrationProfile = new TqRegistrationProfile
                {
                    UniqueLearnerNumber = 1111111111,
                    Firstname = "James",
                    Lastname = "Smith",
                    DateofBirth = new DateTime(2000, 1, 1)
                },
                TqProviderId = 1,
                AcademicYear = 2020,
                StartDate = new DateTime(2021, 2, 28),
                Status = RegistrationPathwayStatus.Active,
                IsPendingWithdrawal = true,
                CreatedOn = new DateTime(2021, 1, 1),
                TqProvider = new TqProvider
                {
                    IsActive = true,
                    TqAwardingOrganisation = new TqAwardingOrganisation
                    {
                        IsActive = true,
                        TlPathway = designSurveyingPathway,
                        TlAwardingOrganisaton = nfce
                    },
                    TlProvider = barnsleyCollegeProvider
                },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                {
                    new()
                    {
                        IsOptedin = true,
                        TlSpecialism = surveyingSpecialism
                    }
                }
            };

            TqRegistrationPathway secondRegPathway = new()
            {
                TqRegistrationProfile = new TqRegistrationProfile
                {
                    UniqueLearnerNumber = 1111111112,
                    Firstname = "Emily",
                    Lastname = "Johnson",
                    DateofBirth = new DateTime(2000, 1, 2)
                },
                TqProviderId = 1,
                AcademicYear = 2020,
                StartDate = new DateTime(2021, 2, 28),
                Status = RegistrationPathwayStatus.Active,
                IsPendingWithdrawal = false,
                CreatedOn = new DateTime(2021, 1, 2),
                TqProvider = new TqProvider
                {
                    IsActive = true,
                    TqAwardingOrganisation = new TqAwardingOrganisation
                    {
                        IsActive = true,
                        TlPathway = designSurveyingPathway,
                        TlAwardingOrganisaton = nfce
                    },
                    TlProvider = barnsleyCollegeProvider
                },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                {
                    new()
                    {
                        IsOptedin = true,
                        TlSpecialism = surveyingSpecialism
                    }
                }
            };

            TqRegistrationPathway thirdRegPathway = new()
            {
                TqRegistrationProfile = new TqRegistrationProfile
                {
                    UniqueLearnerNumber = 1111111113,
                    Firstname = "Oliver",
                    Lastname = "Brown",
                    DateofBirth = new DateTime(2000, 1, 3)
                },
                TqProviderId = 1,
                AcademicYear = 2020,
                StartDate = new DateTime(2021, 2, 28),
                Status = RegistrationPathwayStatus.Withdrawn,
                IsPendingWithdrawal = true,
                CreatedOn = new DateTime(2021, 1, 3),
                TqProvider = new TqProvider
                {
                    IsActive = true,
                    TqAwardingOrganisation = new TqAwardingOrganisation
                    {
                        IsActive = true,
                        TlPathway = designSurveyingPathway,
                        TlAwardingOrganisaton = nfce
                    },
                    TlProvider = barnsleyCollegeProvider
                },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                {
                    new()
                    {
                        IsOptedin = true,
                        TlSpecialism = surveyingSpecialism
                    }
                }
            };

            TqRegistrationPathway fourthRegPathway = new()
            {
                TqRegistrationProfile = new TqRegistrationProfile
                {
                    UniqueLearnerNumber = 1111111114,
                    Firstname = "Sophie",
                    Lastname = "Williams",
                    DateofBirth = new DateTime(2005, 10, 25)
                },
                TqProviderId = 1,
                AcademicYear = 2023,
                StartDate = new DateTime(2023, 9, 1),
                Status = RegistrationPathwayStatus.Active,
                IsPendingWithdrawal = true,
                CreatedOn = new DateTime(2024, 6, 1),
                TqProvider = new TqProvider
                {
                    IsActive = true,
                    TqAwardingOrganisation = new TqAwardingOrganisation
                    {
                        IsActive = true,
                        TlPathway = designSurveyingPathway,
                        TlAwardingOrganisaton = cityAndGuilds
                    },
                    TlProvider = barnsleyCollegeProvider
                },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                {
                    new()
                    {
                        IsOptedin = true,
                        TlSpecialism = surveyingSpecialism
                    }
                }
            };

            DbContext.AddRange(firstRegPathway, secondRegPathway, thirdRegPathway, fourthRegPathway);
            DbContext.SaveChanges();
        }
    }
}