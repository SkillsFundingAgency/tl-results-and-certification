//using FluentAssertions;
//using NSubstitute;
//using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
//using Sfa.Tl.ResultsAndCertification.Application.Services;
//using Sfa.Tl.ResultsAndCertification.Common.Enum;
//using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
//using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
//using Sfa.Tl.ResultsAndCertification.Domain.Models;
//using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
//using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
//using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using IndustryPlacement = Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner.IndustryPlacement;

//namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
//{
//    public class When_GetLearnerAsync_IsCalled : BaseTest<AdminDashboardService>
//    {
//        private AdminDashboardService _adminDashboardService;

//        private AdminLearnerRecord _expectedResult;
//        private AdminLearnerRecord _actualResult;
//        private const int RegistrationPathwayId = 1;

//        public override void Setup()
//        {
//            _expectedResult = new AdminLearnerRecord
//            {
//                RegistrationPathwayId = 1,
//                Uln = 1234567890,
//                Firstname = "John",
//                Lastname = "Smith",
//                DateofBirth = new DateTime(2005, 1, 23),
//                MathsStatus = SubjectStatus.Achieved,
//                EnglishStatus = SubjectStatus.Achieved,
//                OverallCalculationStatus = CalculationStatus.Completed,
//                Pathway = new Pathway
//                {
//                    Id = 3,
//                    LarId = "60369115",
//                    Name = "Building Services Engineering",
//                    StartYear = 2020,
//                    AcademicYear = 2022,
//                    IndustryPlacements = new IndustryPlacement[]
//                   {
//                        new IndustryPlacement
//                        {
//                            Id = 5,
//                            Status = IndustryPlacementStatus.Completed
//                        },
//                   },
//                    Provider = new Provider
//                    {
//                        Id = 2,
//                        Ukprn = 10000536,
//                        Name = "Barnsley College",
//                        DisplayName = "Barnsley College"
//                    }
//                },
//                AwardingOrganisation = new AwardingOrganisation
//                {
//                    Id = 1,
//                    Ukprn = 10009696,
//                    Name = "Ncfe",
//                    DisplayName = "NCFE"
//                }
//            };

//            var today = new DateTime(2023, 1, 1);

//            var repository = Substitute.For<IAdminDashboardRepository>();
//            repository.GetLearnerRecordAsync(Arg.Any<int>()).Returns(_expectedResult);

//            var systemProvider = Substitute.For<ISystemProvider>();
//            systemProvider.UtcToday.Returns(today);

//            var tqRegistrationPathwayRepository = Substitute.For<IRepository<TqRegistrationPathway>>();
//            var commonService = Substitute.For<ICommonService>();

//            _adminDashboardService = new AdminDashboardService(repository, systemProvider, tqRegistrationPathwayRepository, commonService);
//        }

//        public override void Given()
//        {
//        }

//        public override async Task When()
//        {
//            _actualResult = await _adminDashboardService.GetAdminLearnerRecordAsync(RegistrationPathwayId);
//        }

//        [Fact]
//        public void Then_Returns_Expected_Results()
//        {
//            _actualResult.Should().NotBeNull();
//            _actualResult.Should().BeEquivalentTo(_expectedResult);
//            _actualResult.AcademicStartYearsToBe.Count.Should().Be(2);
//            _actualResult.AcademicStartYearsToBe.Should().Contain(new List<int>() { 2021, 2020 });
//            _actualResult.DisplayAcademicYear.Should().BeEquivalentTo("2022 to 2023");
//        }
//    }
//}
