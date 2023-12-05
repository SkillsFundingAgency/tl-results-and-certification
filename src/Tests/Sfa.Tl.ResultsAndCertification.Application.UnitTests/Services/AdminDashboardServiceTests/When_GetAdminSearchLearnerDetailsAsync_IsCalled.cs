using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetAdminSearchLearnerDetailsAsync_IsCalled : BaseTest<AdminDashboardService>
    {
        private AdminDashboardService _adminDashboardService;

        private PagedResponse<AdminSearchLearnerDetail> _expectedResult;
        private PagedResponse<AdminSearchLearnerDetail> _actualResult;

        public override void Setup()
        {
            _expectedResult = new PagedResponse<AdminSearchLearnerDetail>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchLearnerDetail>
                           {
                               new AdminSearchLearnerDetail
                               {
                                   Uln = 1234567890,
                                   Firstname = "Jessica",
                                   Lastname = "Johnson",
                                   Provider = "Barnsley College",
                                   ProviderUkprn = 10000536,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2021
                               }
                           },
                PagerInfo = new Pager(1, 1, 10)
            };

            var repository = Substitute.For<IAdminDashboardRepository>();
            var systemProvider = Substitute.For<ISystemProvider>();

            repository.SearchLearnerDetailsAsync(Arg.Any<AdminSearchLearnerRequest>()).Returns(_expectedResult);
            var mapper = Substitute.For<IMapper>();

            _adminDashboardService = new AdminDashboardService(repository, systemProvider, mapper);
        }

        public override void Given()
        {
        }

        public override async Task When()
        {
            var request = new AdminSearchLearnerRequest { SearchKey = "Johnson" };
            _actualResult = await _adminDashboardService.GetAdminSearchLearnerDetailsAsync(request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}