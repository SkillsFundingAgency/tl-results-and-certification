using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.SearchRegistrationRepositoryTests
{
    public class When_SearchRegistrationDetailsAsync_IsCalled : BaseTest<TqRegistrationPathway>
    {
        private SearchRegistrationRepository _repository;
        private PagedResponse<SearchRegistrationDetail> _actualResult;

        public override void Given()
        {
            SeedLearnerRegistrations();
            _repository = new SearchRegistrationRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(SearchRegistrationRequest request)
        {
            _actualResult = await _repository.SearchRegistrationDetailsAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(SearchRegistrationRequest request, PagedResponse<SearchRegistrationDetail> expected)
        {
            await WhenAsync(request);

            _actualResult.TotalRecords.Should().Be(expected.TotalRecords);
            _actualResult.PagerInfo.Should().BeEquivalentTo(expected.PagerInfo);
            _actualResult.Records.Should().BeEquivalentTo(expected.Records, options => options.Excluding(p => p.RegistrationProfileId));
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "non-existing-key"
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>(),
                           PagerInfo = new Pager(0, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "11"
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2020,
                                   IsWithdrawn = false,
                                   HasResults = true
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "12"
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 12,
                                   Firstname = "Jessica",
                                   Lastname = "Johnson",
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2021,
                                   IsWithdrawn = true,
                                   HasResults = false
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "21"
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2022,
                                   IsWithdrawn = false,
                                   HasResults = true
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "smith"
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2020,
                                   IsWithdrawn = false,
                                   HasResults = true
                               },
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2022,
                                   IsWithdrawn = false,
                                   HasResults = true
                               }
                           },
                           PagerInfo = new Pager(2, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = " smith    "
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2020,
                                   IsWithdrawn = false,
                                   HasResults = true
                               },
                               new()
                               {
                                   Uln = 21,
                                   Firstname = "Peter",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2022,
                                   IsWithdrawn = false,
                                   HasResults = true
                               }
                           },
                           PagerInfo = new Pager(2, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = string.Empty,
                            SelectedAcademicYears = new List<int> { 2020 }
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                               new()
                               {
                                   Uln = 11,
                                   Firstname = "John",
                                   Lastname = "Smith",
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   PathwayName = "Design, Surveying and Planning",
                                   PathwayLarId = "10123456",
                                   AcademicYear = 2020,
                                   IsWithdrawn = false,
                                   HasResults = true
                               }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10009696,
                            SearchKey = "Johnson",
                            SelectedAcademicYears = new List<int> { 2021, 2022 }
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 1,
                           Records = new List<SearchRegistrationDetail>
                           {
                                new()
                                {
                                    Uln = 31,
                                    Firstname = "Eric",
                                    Lastname = "Johnson",
                                    ProviderName = "ncfe-provider-01",
                                    ProviderUkprn = 3,
                                    PathwayName = "Education",
                                    PathwayLarId = "10123457",
                                    AcademicYear = 2021,
                                    IsWithdrawn = true,
                                    HasResults = false
                                }
                           },
                           PagerInfo = new Pager(1, 1, 10)
                        }
                    },
                    new object[]
                    {
                        new SearchRegistrationRequest
                        {
                            AoUkprn = 10011881,
                            SearchKey = "Johnson",
                            SelectedAcademicYears = new List<int> { 2021 },
                            ProviderId = 1
                        },
                        new PagedResponse<SearchRegistrationDetail>
                        {
                           TotalRecords = 3,
                           Records = new List<SearchRegistrationDetail>
                           {
                                new()
                                {
                                    Uln = 12,
                                    Firstname = "Jessica",
                                    Lastname = "Johnson",
                                    ProviderName = "pearson-provider-01",
                                    ProviderUkprn = 1,
                                    PathwayName = "Design, Surveying and Planning",
                                    PathwayLarId = "10123456",
                                    AcademicYear = 2021,
                                    IsWithdrawn = true,
                                    HasResults = false
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

            AssessmentSeries coreAssessmentSeries = SeedAssessmentSeries(ComponentType.Core);
            AssessmentSeries specialismAssessmentSeries = SeedAssessmentSeries(ComponentType.Specialism);

            TqRegistrationProfile pearsonProviderOneLearnerOne = SeedRegistrationProfile(11, "John", "Smith", new DateTime(2000, 1, 1));
            IEnumerable<TqRegistrationPathway> pearsonProviderOneLearnerOneRegPathways = SeedRegistrationPathway(pearsonProviderOneLearnerOne, pearsonProviderOne, 2020, RegistrationPathwayStatus.Active);
            TqPathwayAssessment pearsonProviderOneLearnerOnePathwayAssessment = SeedPathwayAssessment(pearsonProviderOneLearnerOneRegPathways.Last(), coreAssessmentSeries);
            SeedPathwayResult(pearsonProviderOneLearnerOnePathwayAssessment);

            TqRegistrationProfile pearsonProviderOneLearnerTwo = SeedRegistrationProfile(12, "Jessica", "Johnson", new DateTime(2002, 5, 17));
            IEnumerable<TqRegistrationPathway> pearsonProviderOneLearnerTwoRegPathways = SeedRegistrationPathway(pearsonProviderOneLearnerTwo, pearsonProviderOne, 2021, RegistrationPathwayStatus.Withdrawn);
            SeedPathwayAssessment(pearsonProviderOneLearnerTwoRegPathways.Last(), coreAssessmentSeries);

            TqRegistrationProfile pearsonProviderTwoLearnerOne = SeedRegistrationProfile(21, "Peter", "Smith", new DateTime(1999, 8, 6));
            IEnumerable<TqRegistrationPathway> pearsonProviderTwoLearnerOneRegPathways = SeedRegistrationPathway(pearsonProviderTwoLearnerOne, pearsonProviderTwo, 2022, RegistrationPathwayStatus.Withdrawn, RegistrationPathwayStatus.Active);
            TqRegistrationSpecialism pearsonProviderTwoLearnerOneRegSpecialism = SeedRegistrationSpecialism(pearsonProviderTwoLearnerOneRegPathways.Last());
            TqSpecialismAssessment pearsonProviderTwoLearnerOneSpecialismAssessment = SeedSpecialismAssessment(pearsonProviderTwoLearnerOneRegSpecialism, specialismAssessmentSeries);
            SeedSpecialismResult(pearsonProviderTwoLearnerOneSpecialismAssessment);

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

        private IEnumerable<TqRegistrationPathway> SeedRegistrationPathway(TqRegistrationProfile tqRegistrationProfile, TqProvider tqProvider, int academicYear, params RegistrationPathwayStatus[] statuses)
        {
            var results = new List<TqRegistrationPathway>();

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

                TqRegistrationPathway registrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationPathway);
                results.Add(registrationPathway);
            }

            return results;
        }

        private AssessmentSeries SeedAssessmentSeries(ComponentType componentType)
        {
            var assessmentSeries = new AssessmentSeries
            {
                ComponentType = componentType,
                Name = "Summer 2021",
                Description = "Summer 2021",
                StartDate = new DateTime(2020, 11, 26),
                EndDate = new DateTime(2021, 8, 10)
            };

            return AssessmentSeriesDataProvider.CreateAssessmentSeries(DbContext, assessmentSeries);
        }

        private TqPathwayAssessment SeedPathwayAssessment(TqRegistrationPathway registrationPathway, AssessmentSeries assessmentSeries)
        {
            var pathwayAssessment = new TqPathwayAssessment
            {
                TqRegistrationPathwayId = registrationPathway.Id,
                AssessmentSeriesId = assessmentSeries.Id,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                IsOptedin = true
            };

            return PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
        }

        private void SeedPathwayResult(TqPathwayAssessment pathwayAssessment)
        {
            var result = new TqPathwayResult
            {
                TqPathwayAssessment = pathwayAssessment,
                TlLookupId = 1,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true
            };

            TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, result);
        }

        private TqRegistrationSpecialism SeedRegistrationSpecialism(TqRegistrationPathway registrationPathway)
        {
            var registrationSpecialism = new TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = registrationPathway.Id,
                TlSpecialismId = 1,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                IsOptedin = true
            };

            DbContext.Add(registrationSpecialism);
            return registrationSpecialism;
        }

        private TqSpecialismAssessment SeedSpecialismAssessment(TqRegistrationSpecialism registrationSpecialism, AssessmentSeries assessmentSeries)
        {
            var specialismAssessment = new TqSpecialismAssessment
            {
                TqRegistrationSpecialismId = registrationSpecialism.Id,
                AssessmentSeriesId = assessmentSeries.Id,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                IsOptedin = true
            };

            return SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
        }

        private void SeedSpecialismResult(TqSpecialismAssessment specialismAssessment)
        {
            var result = new TqSpecialismResult
            {
                TqSpecialismAssessmentId = specialismAssessment.Id,
                TlLookupId = 10,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = null,
                PrsStatus = PrsStatus.NotSpecified,
                IsOptedin = true
            };

            TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, result);
        }
    }
}