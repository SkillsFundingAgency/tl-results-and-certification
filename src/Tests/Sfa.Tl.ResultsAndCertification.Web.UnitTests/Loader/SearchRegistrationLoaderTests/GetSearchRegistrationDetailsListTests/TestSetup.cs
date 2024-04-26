using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.SearchRegistrationLoaderTests.GetSearchRegistrationDetailsListTests
{
    public abstract class TestSetup : BaseTest<SearchRegistrationLoader>
    {
        protected const long AoUkprn = 123456789;

        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected SearchRegistrationLoader Loader;

        protected SearchRegistrationType SearchRegistrationType { get; set; }

        protected SearchRegistrationCriteriaViewModel CriteriaViewModel = new()
        {
            SearchKey = string.Empty,
            PageNumber = 1,
            Filters = new()
            {
                SelectedProviderId = null,
                AcademicYears = new List<FilterLookupData>
                {
                    new()
                    {
                        Id = 1,
                        Name = "2020 to 2021",
                        IsSelected = true
                    },
                    new()
                    {
                        Id = 2,
                        Name = "2021 to 2022",
                        IsSelected = false
                    },
                    new()
                    {
                        Id = 3,
                        Name = "2022 to 2023",
                        IsSelected = true
                    }
                }
            }
        };

        protected PagedResponse<SearchRegistrationDetail> SearchResponse;
        protected SearchRegistrationDetailsListViewModel Result;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(SearchRegistrationMapper).Assembly));
            IMapper mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new SearchRegistrationLoader(InternalApiClient, mapper);
        }

        public async override Task When()
        {
            Result = await Loader.GetSearchRegistrationDetailsListAsync(AoUkprn, SearchRegistrationType, CriteriaViewModel);
        }

        protected void Given(SearchRegistrationType searchRegistrationType, bool isWithdrawn, bool hasResults)
        {
            SearchRegistrationType = searchRegistrationType;

            SearchResponse = new()
            {
                PagerInfo = new Pager(1, 1, 10),
                TotalRecords = 150,
                Records = new List<SearchRegistrationDetail>
                {
                    new()
                    {
                        RegistrationProfileId = 1,
                        Uln = 1111111111,
                        Firstname = "John",
                        Lastname = "Smith",
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        PathwayName = "Design, Surveying and Planning",
                        PathwayLarId = "60358300",
                        AcademicYear = 2023,
                        IsWithdrawn = isWithdrawn,
                        HasResults = hasResults
                    }
                }
            };

            InternalApiClient.SearchRegistrationDetailsAsync(
                Arg.Is<SearchRegistrationRequest>(c =>
                    c.AoUkprn == AoUkprn
                    && c.ProviderId == null
                    && c.SearchKey == string.Empty
                    && c.PageNumber == 1
                    && new[] { 1, 3 }.All(p => c.SelectedAcademicYears.Contains(p))))
                .Returns(SearchResponse);
        }

        protected void AssertExpectedMethodsCalled()
        {
            InternalApiClient.Received(1).SearchRegistrationDetailsAsync(
                Arg.Is<SearchRegistrationRequest>(c =>
                    c.AoUkprn == AoUkprn
                    && c.ProviderId == null
                    && c.SearchKey == string.Empty
                    && c.PageNumber == 1
                    && new[] { 1, 3 }.All(p => c.SelectedAcademicYears.Contains(p))));
        }

        protected void AssertResultExceptRouteName()
        {
            Result.RegistrationDetails.Should().HaveCount(1);

            SearchRegistrationDetailsViewModel learner = Result.RegistrationDetails[0];
            learner.RegistrationProfileId.Should().Be(1);
            learner.Uln.Should().Be(1111111111);
            learner.LearnerName.Should().Be("John Smith");
            learner.Provider.Should().Be("Barnsley College (10000536)");
            learner.Core.Should().Be("Design, Surveying and Planning (60358300)");
            learner.StartYear.Should().Be("2023 to 2024");

            Result.RegistrationDetails[0].Route.RouteValues.Should()
                .HaveCount(1)
                .And.ContainEquivalentOf(new KeyValuePair<string, string>(Constants.ProfileId, "1"));

            PagerViewModel pagerInfo = Result.PagerInfo;
            pagerInfo.TotalItems.Should().Be(1);
            pagerInfo.CurrentPage.Should().Be(1);
            pagerInfo.PageSize.Should().Be(10);
            pagerInfo.TotalPages.Should().Be(1);
            pagerInfo.StartPage.Should().Be(1);
            pagerInfo.RecordFrom.Should().Be(1);
            pagerInfo.RecordTo.Should().Be(1);

            Result.TotalRecords.Should().Be(150);
        }
    }
}