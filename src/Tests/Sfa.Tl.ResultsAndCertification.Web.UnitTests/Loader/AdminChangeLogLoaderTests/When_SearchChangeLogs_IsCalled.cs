using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminChangeLogLoaderTests
{
    public class When_SearchChangeLogs_IsCalled : AdminChangeLogLoaderBaseTest
    {
        private AdminSearchChangeLogViewModel _expectedResult;
        private AdminSearchChangeLogViewModel _actualResult;

        public override void Given()
        {
            var expectedApiResult = new PagedResponse<AdminSearchChangeLog>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchChangeLog>
                {
                    new AdminSearchChangeLog
                    {
                        ChangeLogId = 1,
                        ChangeType = ChangeType.StartYear,
                        DateAndTimeOfChange = new DateTime(2023, 8, 31, 9, 31, 0),
                        ZendeskTicketID = "1234567-AB",
                        LearnerFirstname = "Jessica",
                        LearnerLastname = "Johnson",
                        Uln = 1234567890,
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        LastUpdatedBy = "admin-user-01"
                    }
                },
                PagerInfo = new Pager(1, 1, 10)
            };

            ApiClient.SearchChangeLogsAsync(Arg.Is<AdminSearchChangeLogRequest>(r => r.SearchKey == "Johnson")).Returns(expectedApiResult);

            _expectedResult = new AdminSearchChangeLogViewModel
            {
                TotalRecords = 150,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 1,
                    StartPage = 1,
                    TotalItems = 1,
                    TotalPages = 1
                },
                ChangeLogDetails = new List<AdminSearchChangeLogDetailsViewModel>
                {
                    new AdminSearchChangeLogDetailsViewModel
                    {
                        ChangeLogId = 1,
                        DateAndTimeOfChange = "31 August 2023 9:31am",
                        ChangeType = (int)ChangeType.StartYear,
                        ChangeRecordLink = new(){
                            Route = RouteConstants.AdminViewChangeStartYearRecord,
                            RouteAttributes = new Dictionary<string, string>(){ { Constants.ChangeLogId, "1" } },
                            Text = "31 August 2023 9:31am"
                        },
                        ZendeskTicketID = "1234567-AB",
                        Learner = "Jessica Johnson (1234567890)",
                        Provider = "Barnsley College (10000536)",
                        LastUpdatedBy = "admin-user-01"
                    }
                },
                SearchCriteriaViewModel = new AdminSearchChangeLogCriteriaViewModel
                {
                    SearchKey = "Johnson",
                    PageNumber = 1
                }
            };
        }

        public override async Task When()
        {
            _actualResult = await Loader.SearchChangeLogsAsync(searchKey: "Johnson", pageNumber: 1);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}