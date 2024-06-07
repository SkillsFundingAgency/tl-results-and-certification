using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminDashboardRepositoryTests
{
    public class When_SearchLearnerDetailsAsync_IsCalled : BaseTest<TqRegistrationPathway>
    {
        private AdminDashboardRepository _adminDashboardRepository;
        private PagedResponse<AdminSearchLearnerDetail> _actualResult;

        public override void Given()
        {
            SeedLearnerRegistrations();
            _adminDashboardRepository = new AdminDashboardRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AdminSearchLearnerRequest request)
        {
            _actualResult = await _adminDashboardRepository.SearchLearnerDetailsAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(AdminSearchLearnerRequest request, PagedResponse<AdminSearchLearnerDetail> expected)
        {
            await WhenAsync(request);

            _actualResult.TotalRecords.Should().Be(expected.TotalRecords);
            _actualResult.PagerInfo.Should().BeEquivalentTo(expected.PagerInfo);
            _actualResult.Records.Should().BeEquivalentTo(expected.Records, options => options.Excluding(p => p.RegistrationPathwayId));
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "non-existing-key"
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>(),
                           PagerInfo = new Pager(0, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "11"
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2020
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "12"
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 12,
                                   Firstname = "Jessica",
                                   Lastname = "Johnson",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2021
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "21"
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2022
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = " 21   "
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2022
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "smith"
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2020
                               },
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2022
                               }
                           },
                           PagerInfo = new Pager(2, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = " smith   "
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2020
                               },
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2022
                               }
                           },
                           PagerInfo = new Pager(2, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = string.Empty,
                            SelectedAwardingOrganisations = new List<int> { 1 } // Pearson
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2020
                               },
                               new()
                               {
                                   Uln = 12,
                                   Firstname = "Jessica",
                                   Lastname = "Johnson",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2021
                               },
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2022
                                }
                           },
                           PagerInfo = new Pager(3, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = string.Empty,
                            SelectedAwardingOrganisations = new List<int> { 1 }, // Pearson
                            SelectedAcademicYears = new List<int> { 2020 }
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   Provider = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2020
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "Johnson",
                            SelectedAcademicYears = new List<int> { 2021, 2022 }
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                                new()
                                {
                                    Uln = 12,
                                    Firstname = "Jessica",
                                    Lastname = "Johnson",
                                    Provider = "pearson-provider-01",
                                    ProviderUkprn = 1,
                                    AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                    AcademicYear = 2021
                                },
                                new()
                                {
                                    Uln = 31,
                                    Firstname = "Eric",
                                    Lastname = "Johnson",
                                    Provider = "ncfe-provider-01",
                                    ProviderUkprn = 3,
                                    AwardingOrganisation = EnumAwardingOrganisation.Ncfe.ToString(),
                                    AcademicYear = 2021
                                }
                           },
                           PagerInfo = new Pager(2, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchLearnerRequest
                        {
                            SearchKey = "Johnson",
                            SelectedAcademicYears = new List<int> { 2021 },
                            ProviderId = 1
                        },
                        new PagedResponse<AdminSearchLearnerDetail>
                        {
                           TotalRecords = 4,
                           Records = new List<AdminSearchLearnerDetail>
                           {
                                new()
                                {
                                    Uln = 12,
                                    Firstname = "Jessica",
                                    Lastname = "Johnson",
                                    Provider = "pearson-provider-01",
                                    ProviderUkprn = 1,
                                    AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                    AcademicYear = 2021
                                }
                           },
                           PagerInfo = new Pager(1, 1, 10)
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

            TqRegistrationProfile pearsonProviderOneLearnerOne = SeedRegistrationProfile(11, "John", "Smith", new DateTime(2000, 1, 1));
            SeedRegistrationPathway(pearsonProviderOneLearnerOne, pearsonProviderOne, 2020, RegistrationPathwayStatus.Active);

            TqRegistrationProfile pearsonProviderOneLearnerTwo = SeedRegistrationProfile(12, "Jessica", "Johnson", new DateTime(2002, 5, 17));
            SeedRegistrationPathway(pearsonProviderOneLearnerTwo, pearsonProviderOne, 2021, RegistrationPathwayStatus.Withdrawn);

            TqRegistrationProfile pearsonProviderTwoLearnerOne = SeedRegistrationProfile(21, "Peter", "Smith", new DateTime(1999, 8, 6));
            SeedRegistrationPathway(pearsonProviderTwoLearnerOne, pearsonProviderTwo, 2022, RegistrationPathwayStatus.Withdrawn, RegistrationPathwayStatus.Active);

            TqAwardingOrganisation ncfe = SeedAwardingOrganisation(EnumAwardingOrganisation.Ncfe);
            TqProvider ncfeProviderOne = SeedProvider(ncfe, 3, "ncfe-provider-01");

            TqRegistrationProfile ncfeProviderOneLearnerOne = SeedRegistrationProfile(31, "Eric", "Johnson", new DateTime(2003, 3, 1));
            SeedRegistrationPathway(ncfeProviderOneLearnerOne, ncfeProviderOne, 2021, RegistrationPathwayStatus.Withdrawn, RegistrationPathwayStatus.Withdrawn);

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

        private void SeedRegistrationPathway(TqRegistrationProfile tqRegistrationProfile, TqProvider tqProvider, int academicYear, params RegistrationPathwayStatus[] statuses)
        {
            foreach (RegistrationPathwayStatus status in statuses)
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

                RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationPathway);
            }
        }
    }
}