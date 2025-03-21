﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminFindNotificationGet
{
    public class When_Cache_Not_Empty : AdminNotificationControllerBaseTest
    {
        private const int PageNumber = 2;

        private readonly AdminFindNotificationCriteriaViewModel _criteriaViewModel = new()
        {
            ActiveFilters = new List<FilterLookupData>
            {
                CreateFilter(1, "Yes", isSelected: true),
                CreateFilter(2, "No")
            }
        };

        private AdminFindNotificationViewModel _viewModel;

        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new AdminFindNotificationViewModel
            {
                SearchCriteria = _criteriaViewModel,
                Details = new List<AdminFindNotificationDetailsViewModel>
                {
                    new()
                    {
                        EndDate = "20 April 2024",
                        Target = "Provider",
                        IsActive = "Yes",
                        NotificationDetailsLink = new ChangeRecordModel
                        {
                            Text = "text",
                            Route = "route",
                            RouteAttributes = new Dictionary<string, string> { ["key"] = "value"}
                        }
                    }
                }
            };

            CacheService.GetAsync<AdminFindNotificationViewModel>(CacheKey).Returns(_viewModel);
            AdminNotificationLoader.SearchNotificationAsync(_criteriaViewModel).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminFindNotificationAsync(PageNumber);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminFindNotificationViewModel>(CacheKey);
            AdminNotificationLoader.DidNotReceive().LoadFilters();
            AdminNotificationLoader.Received(1).SearchNotificationAsync(_criteriaViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<AdminFindNotificationViewModel>();
            model.Should().Be(_viewModel);

            _viewModel.SearchCriteria.PageNumber.Should().Be(PageNumber);
        }
    }
}