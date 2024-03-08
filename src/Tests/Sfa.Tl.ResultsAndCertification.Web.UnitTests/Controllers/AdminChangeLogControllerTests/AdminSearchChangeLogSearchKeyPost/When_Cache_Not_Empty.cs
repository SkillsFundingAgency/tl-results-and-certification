using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchChangeLogSearchKeyPost
{
    public class When_Cache_Not_Empty : AdminChangeLogControllerTestBase
    {
        private readonly AdminSearchChangeLogViewModel _cachedViewModel = new()
        {
            SearchCriteriaViewModel = new AdminSearchChangeLogCriteriaViewModel
            {
                SearchKey = "",
                PageNumber = 1
            },
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
                CurrentPage = 5,
                PageSize = 10,
                RecordFrom = 1539,
                RecordTo = 1540,
                StartPage = 1,
                TotalItems = 2,
                TotalPages = 5
            }
        };

        private readonly AdminSearchChangeLogCriteriaViewModel _viewModel = new()
        {
            SearchKey = "smith",
            PageNumber = 1
        };

        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey).Returns(_cachedViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchChangeLogSearchKeyAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchChangeLogViewModel>(p =>
                p == _cachedViewModel
                && p.SearchCriteriaViewModel.SearchKey == _viewModel.SearchKey
                && p.SearchCriteriaViewModel.PageNumber == _viewModel.PageNumber));

            AdminChangeLogLoader.ReceivedCalls().Should().BeEmpty();
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchChangeLog()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminSearchChangeLog, ("pageNumber", _cachedViewModel.SearchCriteriaViewModel.PageNumber));
        }
    }
}