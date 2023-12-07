using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;
using Xunit;
using static System.Net.WebRequestMethods;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetLearnerAsync_IsCalled : BaseTest<AdminDashboardService>
    {
        private AdminDashboardService _adminDashboardService;

        private AdminLearnerRecord _expectedResult;
        private AdminLearnerRecord _actualResult;
        private int PathwayId = 1;

        public override void Setup()
        {
            var mockRegistrationPathway = Substitute.For<TqRegistrationPathway>();

            _expectedResult = new AdminLearnerRecord
            {
                FirstName = "John",
                LastName = "Smith",
                Uln = 1234567890,
                ProviderName = "Barnsley College",
                AcademicYear = 2022,
                TlevelName = "Building Services Engineering"
            };

            var today = new DateTime(2023, 1, 1);

            var repository = Substitute.For<IAdminDashboardRepository>();
            repository.GetLearnerRecordAsync(Arg.Any<int>()).Returns(mockRegistrationPathway);

            var systemProvider = Substitute.For<ISystemProvider>();
            systemProvider.UtcToday.Returns(today);
            
            var mapper = Substitute.For<IMapper>();
            mapper.Map<AdminLearnerRecord>(mockRegistrationPathway).Returns(_expectedResult);

            _adminDashboardService = new AdminDashboardService(repository, systemProvider, mapper);
        }

        public override void Given()
        {
        }

        public override async Task When()
        {
            _actualResult = await _adminDashboardService.GetAdminLearnerRecordAsync(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}
