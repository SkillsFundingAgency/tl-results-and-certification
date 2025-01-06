using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminNotificationRepositoryTests
{
    public class When_SearchNotifications_IsCalled : BaseTest<Notification>
    {
        private readonly AdminNotificationRepository _repository;

        private PagedResponse<SearchNotificationDetail> _actualResult;

        public When_SearchNotifications_IsCalled()
        {
            _repository = new AdminNotificationRepository(DbContext);
        }

        public override void Given()
        {
            SeedBanners();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AdminSearchNotificationRequest request, DateTime searchDate)
        {
            _actualResult = await _repository.SearchNotificationsAsync(request, () => searchDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(AdminSearchNotificationRequest request, DateTime searchDate, PagedResponse<SearchNotificationDetail> expected)
        {
            await WhenAsync(request, searchDate);

            _actualResult.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new AdminSearchNotificationRequest
                        {
                            SelectedActive = new[] { 1 },
                            PageNumber = null
                        },
                        new DateTime(2024, 3, 10),
                        new PagedResponse<SearchNotificationDetail>
                        {
                            TotalRecords = 3,
                            Records = new List<SearchNotificationDetail>(),
                            PagerInfo = new Pager(0, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchNotificationRequest
                        {
                            SelectedActive = new[] { 1 },
                            PageNumber = null
                        },
                        new DateTime(2024, 2, 1),
                        new PagedResponse<SearchNotificationDetail>
                        {
                            TotalRecords = 3,
                            Records = new List<SearchNotificationDetail>()
                            {
                                new()
                                {
                                    Id = 1,
                                    Title = "title-1",
                                    Target = NotificationTarget.AwardingOrganisation,
                                    End = new DateTime(2024, 2, 1),
                                    IsActive = true
                                },
                                new()
                                {
                                    Id = 2,
                                    Title = "title-2",
                                    Target = NotificationTarget.Provider,
                                    End = new DateTime(2024, 3, 1),
                                    IsActive = true
                                }
                            },
                            PagerInfo = new Pager(2, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchNotificationRequest
                        {
                            SelectedActive = new[] { 2 },
                            PageNumber = null
                        },
                        new DateTime(2024, 2, 1),
                        new PagedResponse<SearchNotificationDetail>
                        {
                            TotalRecords = 3,
                            Records = new List<SearchNotificationDetail>()
                            {
                                new()
                                {
                                    Id = 3,
                                    Title = "title-3",
                                    Target = NotificationTarget.Both,
                                    End = new DateTime(2024, 12, 31),
                                    IsActive = false
                                }
                            },
                            PagerInfo = new Pager(1, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchNotificationRequest
                        {
                            SelectedActive = new[] { 1, 2 },
                            PageNumber = null
                        },
                        new DateTime(2024, 12, 31),
                        new PagedResponse<SearchNotificationDetail>
                        {
                            TotalRecords = 3,
                            Records = new List<SearchNotificationDetail>()
                            {
                                new()
                                {
                                    Id = 1,
                                    Title = "title-1",
                                    Target = NotificationTarget.AwardingOrganisation,
                                    End = new DateTime(2024, 2, 1),
                                    IsActive = false
                                },
                                new()
                                {
                                    Id = 2,
                                    Title = "title-2",
                                    Target = NotificationTarget.Provider,
                                    End = new DateTime(2024, 3, 1),
                                    IsActive = false
                                },
                                new()
                                {
                                    Id = 3,
                                    Title = "title-3",
                                    Target = NotificationTarget.Both,
                                    End = new DateTime(2024, 12, 31),
                                    IsActive = true
                                }
                            },
                            PagerInfo = new Pager(3, 0, 10)
                        }
                    },
                    new object[]
                    {
                        new AdminSearchNotificationRequest
                        {
                            SelectedActive = Enumerable.Empty<int>(),
                            PageNumber = null
                        },
                        new DateTime(2024, 2, 1),
                        new PagedResponse<SearchNotificationDetail>
                        {
                            TotalRecords = 3,
                            Records = new List<SearchNotificationDetail>()
                            {
                                new()
                                {
                                    Id = 1,
                                    Title = "title-1",
                                    Target = NotificationTarget.AwardingOrganisation,
                                    End = new DateTime(2024, 2, 1),
                                    IsActive = true
                                },
                                new()
                                {
                                    Id = 2,
                                    Title = "title-2",
                                    Target = NotificationTarget.Provider,
                                    End = new DateTime(2024, 3, 1),
                                    IsActive = true
                                },
                                new()
                                {
                                    Id = 3,
                                    Title = "title-3",
                                    Target = NotificationTarget.Both,
                                    End = new DateTime(2024, 12, 31),
                                    IsActive = false
                                }
                            },
                            PagerInfo = new Pager(3, 0, 10)
                        }
                    }
                };
            }
        }

        private void SeedBanners()
        {
            var banners = new Notification[]
            {
                new()
                {
                    Id = 1,
                    Title = "title-1",
                    Content = "content-1",
                    Target = NotificationTarget.AwardingOrganisation,
                    Start = new DateTime(2024, 1, 1),
                    End = new DateTime(2024, 2, 1)
                },
                new()
                {
                    Id = 2,
                    Title = "title-2",
                    Content = "content-2",
                    Target = NotificationTarget.Provider,
                    Start = new DateTime(2024, 2, 1),
                    End = new DateTime(2024, 3, 1)
                },
                new()
                {
                    Id = 3,
                    Title = "title-3",
                    Content = "content-3",
                    Target = NotificationTarget.Both,
                    Start = new DateTime(2024, 12, 1),
                    End = new DateTime(2024, 12, 31)
                }
            };

            DbContext.Notification.AddRange(banners);
            DbContext.SaveChanges();
        }
    }
}