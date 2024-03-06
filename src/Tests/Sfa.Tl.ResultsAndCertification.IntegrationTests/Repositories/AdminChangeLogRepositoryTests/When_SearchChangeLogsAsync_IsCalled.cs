using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminChangeLogRepositoryTests
{
    public class When_SearchChangeLogsAsync_IsCalled : BaseTest<ChangeLog>
    {
        private AdminChangeLogRepository _adminchangeLogRepository;
        private PagedResponse<AdminSearchChangeLog> _actualResult;

        public override void Given()
        {
            SeedLearnerRegistrations();
            _adminchangeLogRepository = new AdminChangeLogRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AdminSearchChangeLogRequest request)
        {
            _actualResult = await _adminchangeLogRepository.SearchChangeLogsAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(AdminSearchChangeLogRequest request, PagedResponse<AdminSearchChangeLog> expected)
        {
            await WhenAsync(request);

            _actualResult.TotalRecords.Should().Be(expected.TotalRecords);
            _actualResult.PagerInfo.Should().BeEquivalentTo(expected.PagerInfo);
            _actualResult.Records.Should().BeEquivalentTo(expected.Records);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = string.Empty
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>
                           {
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 1,
                                   DateAndTimeOfChange = new DateTime(2024, 1, 1),
                                   LearnerFirstname = "John",
                                   LearnerLastname = "Smith",
                                   Uln = 1100000000,
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   ZendeskTicketID = "zendesk-ticket-01",
                                   LastUpdatedBy = "admin-user-01"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 2,
                                   DateAndTimeOfChange = new DateTime(2024, 3, 1),
                                   LearnerFirstname = "Jessica",
                                   LearnerLastname = "Johnson",
                                   Uln = 1200000000,
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   ZendeskTicketID = "zendesk-ticket-02",
                                   LastUpdatedBy = "admin-user-02"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 3,
                                   DateAndTimeOfChange = new DateTime(2022, 12, 31),
                                   LearnerFirstname = "Peter",
                                   LearnerLastname = "Smith",
                                   Uln = 2100000000,
                                   ProviderName = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   ZendeskTicketID = "zendesk-ticket-03",
                                   LastUpdatedBy = "admin-user-03"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 4,
                                   DateAndTimeOfChange = new DateTime(2020, 6, 15),
                                   LearnerFirstname = "Eric",
                                   LearnerLastname = "Johnson",
                                   Uln = 3100000000,
                                   ProviderName = "ncfe-provider-01",
                                   ProviderUkprn = 3,
                                   ZendeskTicketID = "zendesk-ticket-04",
                                   LastUpdatedBy = "admin-user-04"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 5,
                                   DateAndTimeOfChange = new DateTime(2024, 4, 6),
                                   LearnerFirstname = "Sue",
                                   LearnerLastname = "Baker",
                                   Uln = 3200000000,
                                   ProviderName = "ncfe-provider-01",
                                   ProviderUkprn = 3,
                                   ZendeskTicketID = "1200000000",
                                   LastUpdatedBy = "admin-user-05"
                               }
                           },
                           PagerInfo = new Pager(5, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = "non-existing-key"
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>(),
                           PagerInfo = new Pager(0, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = "1100000000"
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>
                           {
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 1,
                                   DateAndTimeOfChange = new DateTime(2024, 1, 1),
                                   LearnerFirstname = "John",
                                   LearnerLastname = "Smith",
                                   Uln = 1100000000,
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   ZendeskTicketID = "zendesk-ticket-01",
                                   LastUpdatedBy = "admin-user-01"
                               }
                           },
                           PagerInfo = new Pager(1, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = "smith"
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>
                           {
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 1,
                                   DateAndTimeOfChange = new DateTime(2024, 1, 1),
                                   LearnerFirstname = "John",
                                   LearnerLastname = "Smith",
                                   Uln = 1100000000,
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   ZendeskTicketID = "zendesk-ticket-01",
                                   LastUpdatedBy = "admin-user-01"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 3,
                                   DateAndTimeOfChange = new DateTime(2022, 12, 31),
                                   LearnerFirstname = "Peter",
                                   LearnerLastname = "Smith",
                                   Uln = 2100000000,
                                   ProviderName = "pearson-provider-02",
                                   ProviderUkprn = 2,
                                   ZendeskTicketID = "zendesk-ticket-03",
                                   LastUpdatedBy = "admin-user-03"
                               }
                           },
                           PagerInfo = new Pager(2, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = "zendesk-ticket-04"
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>
                           {
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 4,
                                   DateAndTimeOfChange = new DateTime(2020, 6, 15),
                                   LearnerFirstname = "Eric",
                                   LearnerLastname = "Johnson",
                                   Uln = 3100000000,
                                   ProviderName = "ncfe-provider-01",
                                   ProviderUkprn = 3,
                                   ZendeskTicketID = "zendesk-ticket-04",
                                   LastUpdatedBy = "admin-user-04"
                               }
                           },
                           PagerInfo = new Pager(1, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchChangeLogRequest
                        {
                            SearchKey = "1200000000"
                        },
                        new PagedResponse<AdminSearchChangeLog>
                        {
                           TotalRecords = 5,
                           Records = new List<AdminSearchChangeLog>
                           {
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 2,
                                   DateAndTimeOfChange = new DateTime(2024, 3, 1),
                                   LearnerFirstname = "Jessica",
                                   LearnerLastname = "Johnson",
                                   Uln = 1200000000,
                                   ProviderName = "pearson-provider-01",
                                   ProviderUkprn = 1,
                                   ZendeskTicketID = "zendesk-ticket-02",
                                   LastUpdatedBy = "admin-user-02"
                               },
                               new AdminSearchChangeLog
                               {
                                   ChangeLogId = 5,
                                   DateAndTimeOfChange = new DateTime(2024, 4, 6),
                                   LearnerFirstname = "Sue",
                                   LearnerLastname = "Baker",
                                   Uln = 3200000000,
                                   ProviderName = "ncfe-provider-01",
                                   ProviderUkprn = 3,
                                   ZendeskTicketID = "1200000000",
                                   LastUpdatedBy = "admin-user-05"
                               }
                           },
                           PagerInfo = new Pager(2, 0, 10)
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

            TqRegistrationProfile pearsonProviderOneLearnerOne = SeedRegistrationProfile(1100000000, "John", "Smith", new DateTime(2000, 1, 1));
            TqRegistrationPathway pearsonProviderOneLearnerOneRegPathway = SeedRegistrationPathway(pearsonProviderOneLearnerOne, pearsonProviderOne, 2020, RegistrationPathwayStatus.Active);
            SeedChangeLog(pearsonProviderOneLearnerOneRegPathway, ChangeType.StartYear, "admin-user-01", new DateTime(2024, 1, 1), "zendesk-ticket-01");

            TqRegistrationProfile pearsonProviderOneLearnerTwo = SeedRegistrationProfile(1200000000, "Jessica", "Johnson", new DateTime(2002, 5, 17));
            TqRegistrationPathway pearsonProviderOneLearnerTwoRegPathway = SeedRegistrationPathway(pearsonProviderOneLearnerTwo, pearsonProviderOne, 2021, RegistrationPathwayStatus.Withdrawn);
            SeedChangeLog(pearsonProviderOneLearnerTwoRegPathway, ChangeType.IndustryPlacement, "admin-user-02", new DateTime(2024, 3, 1), "zendesk-ticket-02");

            TqRegistrationProfile pearsonProviderTwoLearnerOne = SeedRegistrationProfile(2100000000, "Peter", "Smith", new DateTime(1999, 8, 6));
            TqRegistrationPathway pearsonProviderTwoLearnerOneRegPathway = SeedRegistrationPathway(pearsonProviderTwoLearnerOne, pearsonProviderTwo, 2022, RegistrationPathwayStatus.Active);
            SeedChangeLog(pearsonProviderTwoLearnerOneRegPathway, ChangeType.AddPathwayResult, "admin-user-03", new DateTime(2022, 12, 31), "zendesk-ticket-03");

            TqAwardingOrganisation ncfe = SeedAwardingOrganisation(EnumAwardingOrganisation.Ncfe);
            TqProvider ncfeProviderOne = SeedProvider(ncfe, 3, "ncfe-provider-01");

            TqRegistrationProfile ncfeProviderOneLearnerOne = SeedRegistrationProfile(3100000000, "Eric", "Johnson", new DateTime(2003, 3, 1));
            TqRegistrationPathway ncfeProviderOneLearnerOneRegPathway = SeedRegistrationPathway(ncfeProviderOneLearnerOne, ncfeProviderOne, 2021, RegistrationPathwayStatus.Withdrawn);
            SeedChangeLog(ncfeProviderOneLearnerOneRegPathway, ChangeType.AssessmentEntryAdd, "admin-user-04", new DateTime(2020, 6, 15), "zendesk-ticket-04");

            TqRegistrationProfile ncfeProviderOneLearnerTwo = SeedRegistrationProfile(3200000000, "Sue", "Baker", new DateTime(2005, 11, 30));
            TqRegistrationPathway ncfeProviderOneLearnerTwoRegPathway = SeedRegistrationPathway(ncfeProviderOneLearnerTwo, ncfeProviderOne, 2023, RegistrationPathwayStatus.Active);
            SeedChangeLog(ncfeProviderOneLearnerTwoRegPathway, ChangeType.AddSpecialismResult, "admin-user-05", new DateTime(2024, 4, 6), "1200000000");

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

        private void SeedChangeLog(TqRegistrationPathway registrationPathway, ChangeType changeType, string name, DateTime createdOn, string zendeskTicketID)
        {
            var changeLog = new ChangeLog
            {
                TqRegistrationPathwayId = registrationPathway.Id,
                ChangeType = (int)changeType,
                Details = string.Empty,
                Name = name,
                CreatedOn = createdOn,
                ReasonForChange = string.Empty,
                ZendeskTicketID = zendeskTicketID,
                TqRegistrationPathway = registrationPathway
            };

            DbContext.Add(changeLog);
        }
    }
}