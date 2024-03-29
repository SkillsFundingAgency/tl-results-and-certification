﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetailsGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private SearchCriteriaViewModel _searchCriteria;
        private SearchLearnerFiltersViewModel _searchFilters;
        private SearchLearnerDetailsListViewModel _searchLearnersList;

        public override void Given()
        {
            AcademicYear = 2020;
            _searchCriteria = null;

            _searchFilters = new SearchLearnerFiltersViewModel
            {
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                    new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                },
                Tlevels = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 1, Name = "Design, Survey and Planning", IsSelected = false }, 
                    new FilterLookupData { Id = 2, Name = "Health", IsSelected = false }
                },
                Status = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 1, Name = "English level", IsSelected = false },
                    new FilterLookupData { Id = 2, Name = "Maths level", IsSelected = false },
                    new FilterLookupData { Id = 3, Name = "Industry placement", IsSelected = false },
                    new FilterLookupData { Id = 4, Name = "All incomplete records", IsSelected = false }
                }
            };
            TrainingProviderLoader.GetSearchLearnerFiltersAsync(ProviderUkprn).Returns(_searchFilters);

            _searchLearnersList = new SearchLearnerDetailsListViewModel
            {
                TotalRecords = 2,
                SearchLearnerDetailsList = new List<SearchLearnerDetailsViewModel>
                {
                    new SearchLearnerDetailsViewModel
                    {
                        ProfileId = 1,
                        LearnerName = "John Smith",
                        Uln = 1234567890,
                        StartYear = "2020 to 2021",
                        TlevelName = "Design, Surveying and Planning for Construction",
                        IsEnglishAdded = true,
                        IsMathsAdded = true,
                        IsIndustryPlacementAdded = true
                    }
                }
            };

            CacheService.GetAsync<SearchCriteriaViewModel>(CacheKey).Returns(_searchCriteria);

            _searchCriteria = new SearchCriteriaViewModel
            {
                AcademicYear = AcademicYear,
                PageNumber = PageNumber,
                SearchLearnerFilters = null
            };
                        
            TrainingProviderLoader.SearchLearnerDetailsAsync(ProviderUkprn, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == _searchCriteria.AcademicYear && x.PageNumber == _searchCriteria.PageNumber && x.IsSearchKeyApplied == _searchCriteria.IsSearchKeyApplied)).Returns(_searchLearnersList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchCriteriaViewModel>(CacheKey);
            TrainingProviderLoader.Received(1).GetSearchLearnerFiltersAsync(ProviderUkprn);
            TrainingProviderLoader.Received(1).SearchLearnerDetailsAsync(ProviderUkprn, Arg.Is<SearchCriteriaViewModel>(x => x.AcademicYear == _searchCriteria.AcademicYear && x.PageNumber == _searchCriteria.PageNumber && x.IsSearchKeyApplied == _searchCriteria.IsSearchKeyApplied));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegisteredLearnersViewModel;

            model.Should().NotBeNull();
            model.SearchCriteria.Should().NotBeNull();

            var searchFilters = model.SearchCriteria.SearchLearnerFilters;
            searchFilters.Should().NotBeNull();
            
            // Academic Year
            searchFilters.AcademicYears.Should().NotBeNull();
            searchFilters.AcademicYears.Should().HaveCount(_searchFilters.AcademicYears.Count);
            searchFilters.AcademicYears.Should().BeEquivalentTo(_searchFilters.AcademicYears);

            // Tlevels
            searchFilters.Tlevels.Should().NotBeNull();
            searchFilters.Tlevels.Should().HaveCount(_searchFilters.Tlevels.Count);
            searchFilters.Tlevels.Should().BeEquivalentTo(_searchFilters.Tlevels);

            // Status
            searchFilters.Status.Should().NotBeNull();
            searchFilters.Status.Should().HaveCount(_searchFilters.Status.Count);
            searchFilters.Status.Should().BeEquivalentTo(_searchFilters.Status);

            model.SearchLearnerDetailsList.TotalRecords.Should().Be(_searchLearnersList.TotalRecords);
            model.SearchLearnerDetailsList.SearchLearnerDetailsList.Count.Should().Be(1);

            var actualLearner = model.SearchLearnerDetailsList.SearchLearnerDetailsList.First();
            var expectedLearner = _searchLearnersList.SearchLearnerDetailsList.First();
            actualLearner.ProfileId.Should().Be(expectedLearner.ProfileId);
            actualLearner.LearnerName.Should().Be(expectedLearner.LearnerName);
            actualLearner.StartYear.Should().Be(expectedLearner.StartYear);
            actualLearner.TlevelName.Should().Be(expectedLearner.TlevelName);
            actualLearner.IsEnglishAdded.Should().Be(expectedLearner.IsEnglishAdded);
            actualLearner.IsMathsAdded.Should().Be(expectedLearner.IsMathsAdded);
            actualLearner.IsIndustryPlacementAdded.Should().Be(expectedLearner.IsIndustryPlacementAdded);
            actualLearner.IsStatusCompleted.Should().BeTrue();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Search_Learner_Records);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.SearchLearnerRecord);
        }
    }
}
