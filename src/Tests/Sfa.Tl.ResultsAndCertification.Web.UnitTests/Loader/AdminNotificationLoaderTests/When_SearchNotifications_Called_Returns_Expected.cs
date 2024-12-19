using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public class When_SearchNotifications_Called_Returns_Expected : AdminNotificationLoaderBaseTest
    {
        private readonly AdminFindNotificationCriteriaViewModel _criteria = new()
        {
            ActiveFilters = new List<FilterLookupData>
                {
                    new()
                    {
                        Id = (int)ActiveFilter.Yes,
                        Name = ActiveFilter.Yes.ToString(),
                        IsSelected = true
                    },
                    new()
                    {
                        Id = (int)ActiveFilter.No,
                        Name = ActiveFilter.No.ToString(),
                        IsSelected = false
                    }
                }
        };

        private readonly PagedResponse<SearchNotificationDetail> _apiResponse = new()
        {
            Records = new[]
            {
                new SearchNotificationDetail
                {
                    Id = 1,
                    Title = "title",
                    Target = NotificationTarget.AwardingOrganisation,
                    End = new DateTime(2024, 12, 31),
                    IsActive = true
                }
            },
            TotalRecords = 1,
            PagerInfo = new Pager(1, 1, 10)
        };

        private AdminFindNotificationViewModel _result;

        public override void Given()
        {
            ApiClient
                .SearchNotificationsAsync(Arg.Is<AdminSearchNotificationRequest>((r => r.SelectedActive.Contains((int)ActiveFilter.Yes))))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SearchNotificationAsync(_criteria);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).SearchNotificationsAsync(Arg.Is<AdminSearchNotificationRequest>(r => r.SelectedActive.Contains((int)ActiveFilter.Yes)));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            SearchNotificationDetail notificationDetail = _apiResponse.Records.First();

            _result.Details.Should().HaveCount(1);
            _result.Details[0].IsActive.Should().Be(AdminFindNotification.Label_Yes);
            _result.Details[0].Target.Should().Be("Awarding organisation");

            ChangeRecordModel link = _result.Details[0].NotificationDetailsLink;
            link.Text.Should().Be(notificationDetail.Title);
            link.Route.Should().Be(RouteConstants.AdminNotificationDetails);
            link.RouteAttributes.Should().HaveCount(1);
            link.RouteAttributes.Should().Contain(Constants.NotificationId, notificationDetail.Id.ToString());

            _result.TotalRecords.Should().Be(1);

            PagerViewModel pagerInfo = _result.Pagination.PagerInfo;
            pagerInfo.TotalItems.Should().Be(1);
            pagerInfo.CurrentPage.Should().Be(1);
            pagerInfo.PageSize.Should().Be(10);
            pagerInfo.TotalPages.Should().Be(1);
            pagerInfo.StartPage.Should().Be(1);
            pagerInfo.RecordFrom.Should().Be(1);
            pagerInfo.RecordTo.Should().Be(1);
        }
    }
}