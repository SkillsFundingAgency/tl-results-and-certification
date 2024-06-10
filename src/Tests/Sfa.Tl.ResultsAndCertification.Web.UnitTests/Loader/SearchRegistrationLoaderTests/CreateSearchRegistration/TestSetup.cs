using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.SearchRegistrationLoaderTests.CreateSearchRegistration
{
    public abstract class TestSetup : BaseTest<SearchRegistrationLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected SearchRegistrationLoader Loader;

        protected SearchRegistrationFilters Filters = new()
        {
            AcademicYears = new FilterLookupData[]
            {
                new()
                {
                    Id = 1,
                    Name = "2020 to 2021",
                    IsSelected = false
                },
                new()
                {
                    Id = 2,
                    Name = "2021 to 2022",
                    IsSelected = false
                }
            }
        };

        protected SearchRegistrationFiltersViewModel FiltersViewModel = new()
        {
            Search = string.Empty,
            SelectedProviderId = null,
            AcademicYears = new FilterLookupData[]
            {
                new()
                {
                    Id = 1,
                    Name = "2020 to 2021",
                    IsSelected = false
                },
                new()
                {
                    Id = 2,
                    Name = "2021 to 2022",
                    IsSelected = false
                }
            }
        };

        protected SearchRegistrationType SearchRegistrationType { get; set; }
        protected SearchRegistrationViewModel Result;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(SearchRegistrationMapper).Assembly));
            IMapper mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new SearchRegistrationLoader(InternalApiClient, mapper);
        }

        public async override Task When()
        {
            Result = await Loader.CreateSearchRegistration(SearchRegistrationType);
        }

        protected void Given(SearchRegistrationType searchRegistrationType)
        {
            SearchRegistrationType = searchRegistrationType;
            InternalApiClient.GetSearchRegistrationFiltersAsync().Returns(Filters);
        }

        protected void AssertExpectedMethodsCalled()
        {
            InternalApiClient.Received(1).GetSearchRegistrationFiltersAsync();
        }
    }
}