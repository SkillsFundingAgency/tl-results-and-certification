using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminChangeLogServiceTests
{
    public class When_SearchChangeLogs_IsCalled : AdminChangeLogServiceBaseTest
    {
        private PagedResponse<AdminSearchChangeLog> _expectedResult;
        private PagedResponse<AdminSearchChangeLog> _actualResult;

        private const string Surname = "Johnson";

        public override void Given()
        {
            _expectedResult = new PagedResponse<AdminSearchChangeLog>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchChangeLog>
                {
                    new AdminSearchChangeLog
                    {
                        ChangeLogId = 1,
                        DateAndTimeOfChange = new DateTime(2024, 1, 1),
                        ZendeskTicketID = "1234567-AB",
                        LearnerFirstname = "Jessica",
                        LearnerLastname = Surname,
                        Uln = 1234567890,
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        LastUpdatedBy = "admin-user-01"
                    }
                },
                PagerInfo = new Pager(1, 1, 10)
            };

            AdminChangeLogRepository.SearchChangeLogsAsync(Arg.Is<AdminSearchChangeLogRequest>(r => r.SearchKey == Surname)).Returns(_expectedResult);
        }

        public override async Task When()
        {
            var request = new AdminSearchChangeLogRequest { SearchKey = Surname };
            _actualResult = await AdminChangeLogService.SearchChangeLogsAsync(request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}