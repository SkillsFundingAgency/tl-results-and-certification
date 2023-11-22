using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminSearchLearnerFilters
{
    public class When_Called : BaseTest<AdminDashboardLoader>
    {
        private IResultsAndCertificationInternalApiClient _internalApiClient;
        private AdminDashboardLoader Loader;

        private AdminSearchLearnerFiltersViewModel _expectedResult;
        private AdminSearchLearnerFiltersViewModel _actualResult;

        public override void Setup()
        {
            _internalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminDashboardLoader(_internalApiClient, mapper);
        }

        public override void Given()
        {
            var expectedApiResult = new AdminSearchLearnerFilters
            {
                AwardingOrganisations = new List<AdminFilter>
                {
                    new AdminFilter { Id = 1, Name = "Ncfe", IsSelected = false },
                    new AdminFilter { Id = 2, Name = "Pearson", IsSelected = false }
                },
                AcademicYears = new List<AdminFilter>
                {
                    new AdminFilter { Id = 1, Name = "2020 to 2021", IsSelected = false }
                }
            };

            _internalApiClient.GetAdminSearchLearnerFiltersAsync().Returns(expectedApiResult);

            _expectedResult = new AdminSearchLearnerFiltersViewModel
            {
                AwardingOrganisations = new List<AdminFilter>
                {
                    new AdminFilter { Id = 1, Name = "Ncfe", IsSelected = false },
                    new AdminFilter { Id = 2, Name = "Pearson", IsSelected = false }
                },
                AcademicYears = new List<AdminFilter>
                {
                    new AdminFilter { Id = 1, Name = "2020 to 2021", IsSelected = false }
                }
            };
        }

        public override async Task When()
        {
            _actualResult = await Loader.GetAdminSearchLearnerFiltersAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}