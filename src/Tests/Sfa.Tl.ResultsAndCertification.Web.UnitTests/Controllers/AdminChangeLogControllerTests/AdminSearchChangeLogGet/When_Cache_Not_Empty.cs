using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchChangeLogGet
{
    public class When_Cache_Not_Empty : AdminChangeLogControllerTestBase
    {
        private const int PageNumber = 5;

        private AdminSearchChangeLogViewModel _cachedViewModel;
        private AdminSearchChangeLogViewModel _viewModel;

        private IActionResult _result;

        public override void Given()
        {
            _cachedViewModel = new()
            {
                SearchCriteriaViewModel = new AdminSearchChangeLogCriteriaViewModel
                {
                    PageNumber = PageNumber,
                    SearchKey = "smith"
                }
            };

            _viewModel = new()
            {
                SearchCriteriaViewModel = _cachedViewModel.SearchCriteriaViewModel,
                ChangeLogDetails = new List<AdminSearchChangeLogDetailsViewModel>
                {
                    new AdminSearchChangeLogDetailsViewModel
                    {
                        ChangeLogId = 1,
                        DateAndTimeOfChange = "31 August 2024 9:30am",
                        Learner = "John Smith (1234567890)",
                        Provider = "Bath College (10001465)",
                        ZendeskTicketID = "1234567-AB",
                        LastUpdatedBy = "DfE Admin 01"
                    },
                    new AdminSearchChangeLogDetailsViewModel
                    {
                        ChangeLogId = 154,
                        DateAndTimeOfChange = "1 April 2021 5:15pm",
                        Learner = "Sue Baker (1122334455)",
                        Provider = "St Thomas More Catholic School Blaydon (10036413)",
                        ZendeskTicketID = "1726354-ZX",
                        LastUpdatedBy = "DfE Admin 02"
                    }
                },
                TotalRecords = 1540,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = PageNumber,
                    PageSize = 10,
                    RecordFrom = 1539,
                    RecordTo = 1540,
                    StartPage = 1,
                    TotalItems = 2,
                    TotalPages = 5
                }
            };

            AdminSearchChangeLogCriteriaViewModel criteria = _viewModel.SearchCriteriaViewModel;

            CacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey).Returns(_cachedViewModel);
            AdminChangeLogLoader.SearchChangeLogsAsync(criteria.SearchKey, criteria.PageNumber).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchChangeLogAsync(PageNumber);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminSearchChangeLogCriteriaViewModel criteria = _cachedViewModel.SearchCriteriaViewModel;

            CacheService.Received(1).GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            AdminChangeLogLoader.Received(1).SearchChangeLogsAsync(criteria.SearchKey, criteria.PageNumber);
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = _result.ShouldBeViewResult<AdminSearchChangeLogViewModel>();
            model.Should().BeEquivalentTo(_viewModel);
        }
    }
}