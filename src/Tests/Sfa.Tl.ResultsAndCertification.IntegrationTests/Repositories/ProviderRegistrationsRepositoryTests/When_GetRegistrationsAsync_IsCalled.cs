using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.ProviderRegistrationsRepositoryTests
{
    public class When_GetRegistrationsAsync_IsCalled : BaseTest<TqRegistrationPathway>
    {
        private ProviderRegistrationsRepository _repository;
        private IList<TqRegistrationPathway> _actualResult;

        public override void Given()
        {
            SeedLearnerRegistrations();
            _repository = new ProviderRegistrationsRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, int startYear)
        {
            _actualResult = await _repository.GetRegistrationsAsync(providerUkprn, startYear);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, int startYear, IList<TqRegistrationPathway> expected)
        {
            await WhenAsync(providerUkprn, startYear);

            _actualResult.Should().HaveCount(expected.Count);

            for (int i = 0; i < _actualResult.Count; i++)
            {
                _actualResult[i].AcademicYear.Should().Be(expected[i].AcademicYear);

                _actualResult[i].TqRegistrationProfile.UniqueLearnerNumber.Should().Be(expected[i].TqRegistrationProfile.UniqueLearnerNumber);
                _actualResult[i].TqRegistrationProfile.Firstname.Should().Be(expected[i].TqRegistrationProfile.Firstname);
                _actualResult[i].TqRegistrationProfile.Lastname.Should().Be(expected[i].TqRegistrationProfile.Lastname);
                _actualResult[i].TqRegistrationProfile.DateofBirth.Should().Be(expected[i].TqRegistrationProfile.DateofBirth);
                _actualResult[i].TqRegistrationProfile.EnglishStatus.Should().Be(expected[i].TqRegistrationProfile.EnglishStatus);
                _actualResult[i].TqRegistrationProfile.MathsStatus.Should().Be(expected[i].TqRegistrationProfile.MathsStatus);

                _actualResult[i].IndustryPlacements.First().Status.Should().Be(expected[i].IndustryPlacements.First().Status);

                _actualResult[i].TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle.Should().Be(expected[i].TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle);
                _actualResult[i].TqProvider.TqAwardingOrganisation.TlPathway.Name.Should().Be(expected[i].TqProvider.TqAwardingOrganisation.TlPathway.Name);
                _actualResult[i].TqProvider.TqAwardingOrganisation.TlPathway.LarId.Should().Be(expected[i].TqProvider.TqAwardingOrganisation.TlPathway.LarId);

                _actualResult[i].TqRegistrationSpecialisms.First().TlSpecialism.LarId.Should().Be(expected[i].TqRegistrationSpecialisms.First().TlSpecialism.LarId);
                _actualResult[i].TqRegistrationSpecialisms.First().TlSpecialism.Name.Should().Be(expected[i].TqRegistrationSpecialisms.First().TlSpecialism.Name);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        999,
                        2020,
                        new List<TqRegistrationPathway>()
                    },
                    new object[]
                    {
                        1,
                        2025,
                        new List<TqRegistrationPathway>()
                    },
                    new object[]
                    {
                       1,
                       2020,
                        new List<TqRegistrationPathway>
                        {
                            new()
                            {
                                AcademicYear = 2020,
                                TqRegistrationProfile = new TqRegistrationProfile
                                {
                                    UniqueLearnerNumber = 11,
                                    Firstname = "John",
                                    Lastname = "Smith",
                                    DateofBirth = new DateTime(2000, 1, 1),
                                    EnglishStatus = SubjectStatus.Achieved,
                                    MathsStatus = SubjectStatus.Achieved
                                },
                                IndustryPlacements = new List<IndustryPlacement>
                                {
                                    new()
                                    {
                                        Status = IndustryPlacementStatus.Completed
                                    }
                                },
                                TqProvider = new TqProvider
                                {
                                    TqAwardingOrganisation = new TqAwardingOrganisation
                                    {
                                        TlPathway = new TlPathway
                                        {
                                            TlevelTitle = "T Level in Digital Business Services",
                                            Name = "Digital Business Services",
                                            LarId = "10723456"
                                        }
                                    }
                                },
                                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                                {
                                    new()
                                    {
                                        TlSpecialism = new TlSpecialism
                                        {
                                            Name = "Digital Support",
                                            LarId = "ZTLOS012"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new object[]
                    {
                       2,
                       2021,
                        new List<TqRegistrationPathway>
                        {
                            new()
                            {
                                AcademicYear = 2021,
                                TqRegistrationProfile = new TqRegistrationProfile
                                {
                                    UniqueLearnerNumber = 12,
                                    Firstname = "Jessica",
                                    Lastname = "Johnson",
                                    DateofBirth = new DateTime(2002, 5, 17),
                                    EnglishStatus = SubjectStatus.NotAchieved,
                                    MathsStatus = SubjectStatus.Achieved
                                },
                                IndustryPlacements = new List<IndustryPlacement>
                                {
                                    new()
                                    {
                                        Status = IndustryPlacementStatus.NotCompleted
                                    }
                                },
                                TqProvider = new TqProvider
                                {
                                    TqAwardingOrganisation = new TqAwardingOrganisation
                                    {
                                        TlPathway = new TlPathway
                                        {
                                            TlevelTitle = "T Level in Digital Business Services",
                                            Name = "Digital Business Services",
                                            LarId = "10723456"
                                        }
                                    }
                                },
                                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                                {
                                    new()
                                    {
                                        TlSpecialism = new TlSpecialism
                                        {
                                            Name = "Data Technician",
                                            LarId = "ZTLOS009"
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }

        private void SeedLearnerRegistrations()
        {
            TqAwardingOrganisation pearson = SeedAwardingOrganisation(EnumAwardingOrganisation.Pearson);

            TqProvider pearsonProviderOne = SeedProvider(pearson, 1, "pearson-provider-01");
            TqProvider pearsonProviderTwo = SeedProvider(pearson, 2, "pearson-provider-02");
            SeedDigitalBusinessServicesPathway(pearson);

            TqRegistrationProfile pearsonProviderOneLearnerOne = SeedRegistrationProfile(11, "John", "Smith", new DateTime(2000, 1, 1), SubjectStatus.Achieved, SubjectStatus.Achieved);
            TqRegistrationPathway pearsonProviderOneLearnerOneRegPathway = SeedRegistrationPathway(pearsonProviderOneLearnerOne, pearsonProviderOne, 2020);
            SeedRegistrationSpecialism(pearsonProviderOneLearnerOneRegPathway, "ZTLOS012", "Digital Support");
            SeedIndustryPlacement(pearsonProviderOneLearnerOneRegPathway, IndustryPlacementStatus.Completed);

            TqRegistrationProfile pearsonProviderTwoLearnerTwo = SeedRegistrationProfile(12, "Jessica", "Johnson", new DateTime(2002, 5, 17), SubjectStatus.NotAchieved, SubjectStatus.Achieved);
            TqRegistrationPathway pearsonProviderOneLearnerTwoRegPathway = SeedRegistrationPathway(pearsonProviderTwoLearnerTwo, pearsonProviderTwo, 2021);
            SeedRegistrationSpecialism(pearsonProviderOneLearnerTwoRegPathway, "ZTLOS009", "Data Technician");
            SeedIndustryPlacement(pearsonProviderOneLearnerTwoRegPathway, IndustryPlacementStatus.NotCompleted);

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

        private void SeedDigitalBusinessServicesPathway(TqAwardingOrganisation awardingOrganisation)
        {
            TlPathway pathway = new()
            {
                TlevelTitle = "T Level in Digital Business Services",
                Name = "Digital Business Services",
                LarId = "10723456",
                StartYear = 2020
            };

            awardingOrganisation.TlPathway = pathway;

            DbContext.Add(pathway);
        }

        private TqRegistrationProfile SeedRegistrationProfile(long uln, string firstName, string lastName, DateTime dob, SubjectStatus englishStatus, SubjectStatus mathsStatus)
        {
            var tqRegistrationProfile = new TqRegistrationProfile
            {
                UniqueLearnerNumber = uln,
                Firstname = firstName,
                Lastname = lastName,
                DateofBirth = dob,
                EnglishStatus = englishStatus,
                MathsStatus = mathsStatus
            };

            return RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, tqRegistrationProfile);
        }

        private TqRegistrationPathway SeedRegistrationPathway(TqRegistrationProfile tqRegistrationProfile, TqProvider tqProvider, int academicYear)
        {
            var tqRegistrationPathway = new TqRegistrationPathway
            {
                TqRegistrationProfileId = tqRegistrationProfile.Id,
                TqProviderId = tqProvider.Id,
                AcademicYear = academicYear,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                Status = RegistrationPathwayStatus.Active
            };

            return RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationPathway);
        }

        private void SeedIndustryPlacement(TqRegistrationPathway registrationPathway, IndustryPlacementStatus industryPlacementStatus)
        {
            var industryPlacement = new IndustryPlacement
            {
                Status = industryPlacementStatus
            };

            registrationPathway.IndustryPlacements = new List<IndustryPlacement> { industryPlacement };
            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, industryPlacement);
        }

        private TqRegistrationSpecialism SeedRegistrationSpecialism(TqRegistrationPathway registrationPathway, string specialismLarId, string specialismName)
        {
            var registrationSpecialism = new TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = registrationPathway.Id,
                TlSpecialism = new TlSpecialism
                {
                    LarId = specialismLarId,
                    Name = specialismName
                },
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                IsOptedin = true
            };

            DbContext.Add(registrationSpecialism);
            return registrationSpecialism;
        }
    }
}