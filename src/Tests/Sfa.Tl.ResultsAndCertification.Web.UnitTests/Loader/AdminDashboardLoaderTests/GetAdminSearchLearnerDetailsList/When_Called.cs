using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminSearchLearnerDetailsList
{
    public class When_Called : BaseTest<AdminDashboardLoader>
    {
        private IResultsAndCertificationInternalApiClient _internalApiClient;
        private AdminDashboardLoader Loader;

        private AdminSearchLearnerDetailsListViewModel _expectedResult;
        private AdminSearchLearnerDetailsListViewModel _actualResult;

        public override void Setup()
        {
            _internalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminDashboardLoader(_internalApiClient, mapper);
        }

        public override void Given()
        {
            var expectedApiResult = new PagedResponse<AdminSearchLearnerDetail>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchLearnerDetail>
                           {
                               new AdminSearchLearnerDetail
                               {
                                   RegistrationPathwayId = 99,
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

            _internalApiClient.GetAdminSearchLearnerDetailsAsync(Arg.Any<AdminSearchLearnerRequest>()).Returns(expectedApiResult);

            _expectedResult = new AdminSearchLearnerDetailsListViewModel
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
                LearnerDetails = new List<AdminSearchLearnerDetailsViewModel>
                {
                    new AdminSearchLearnerDetailsViewModel
                    {
                        Uln = 1234567890,
                        LearnerName = "Jessica Johnson",
                        Provider = "Barnsley College (10000536)",
                        StartYear = "2021 to 2022",
                        AwardingOrganisation = "Pearson",
                        RegistrationPathwayId = 99
                    }
                }
            };
        }

        public override async Task When()
        {
            var searchCriteria = new AdminSearchLearnerCriteriaViewModel
            {
                SearchKey = "Johnson",
                PageNumber = 1
            };

            _actualResult = await Loader.GetAdminSearchLearnerDetailsListAsync(searchCriteria);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}