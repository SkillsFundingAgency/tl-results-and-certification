using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchChangeLogGet
{
    public class When_Cache_Empty : AdminChangeLogControllerTestBase
    {
        private AdminSearchChangeLogViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            string _dateAndTimeOfChange = "31 August 2024 9:30am";
            int _changeLogId = 1;

            _viewModel = new()
            {
                SearchCriteriaViewModel = new AdminSearchChangeLogCriteriaViewModel
                {
                    PageNumber = 1,
                    SearchKey = string.Empty
                },
                ChangeLogDetails = new List<AdminSearchChangeLogDetailsViewModel>
                {
                    new AdminSearchChangeLogDetailsViewModel
                    {
                        ChangeLogId = _changeLogId,
                        ChangeType = (int)ChangeType.StartYear,
                        DateAndTimeOfChange = _dateAndTimeOfChange,
                        Learner = "John Smith (1234567890)",
                        Provider = "Bath College (10001465)",
                        ZendeskTicketID = "1234567-AB",
                        LastUpdatedBy = "DfE Admin",
                        ChangeRecordLink = new ViewComponents.ChangeRecordLink.ChangeRecordModel(){
                            Text = _dateAndTimeOfChange,
                            Route = RouteConstants.AdminViewChangeStartYearRecord,
                            RouteAttributes = new Dictionary<string, string>(){ { Constants.ChangeLogId, _changeLogId.ToString() } },

                            }
                    }
                },
                TotalRecords = 1,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 1,
                    StartPage = 1,
                    TotalItems = 1,
                    TotalPages = 1
                }
            };

            CacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey).ReturnsNull();
            AdminChangeLogLoader.SearchChangeLogsAsync().Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchChangeLogAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            AdminChangeLogLoader.Received(1).SearchChangeLogsAsync();
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = _result.ShouldBeViewResult<AdminSearchChangeLogViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            var changelog = model.ChangeLogDetails.FirstOrDefault();
            KeyValuePair<string, string> routeAttributes = new KeyValuePair<string, string>(Constants.ChangeLogId, changelog.ChangeLogId.ToString());

            changelog.ChangeRecordLink.Route.Should().Be(RouteConstants.AdminViewChangeStartYearRecord);
            changelog.ChangeRecordLink.Text.Should().Be(changelog.DateAndTimeOfChange);
            changelog.ChangeRecordLink.RouteAttributes.Should().NotBeEmpty();
            changelog.ChangeRecordLink.RouteAttributes.Should().Contain(routeAttributes);
        }
    }
}