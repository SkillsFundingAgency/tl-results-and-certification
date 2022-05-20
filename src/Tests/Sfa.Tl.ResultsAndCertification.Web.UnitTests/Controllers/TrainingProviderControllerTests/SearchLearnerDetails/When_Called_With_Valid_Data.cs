using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private SearchLearnerDetailsListViewModel _searchLearnersList;

        public override void Given()
        {
            AcademicYear = 2020;

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

            TrainingProviderLoader.SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear).Returns(_searchLearnersList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as SearchLearnerDetailsListViewModel;

            model.Should().NotBeNull();
            model.TotalRecords.Should().Be(_searchLearnersList.TotalRecords);
            model.SearchLearnerDetailsList.Count.Should().Be(1);

            var actualLearner = model.SearchLearnerDetailsList.First();
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
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Manage_Learner_Records);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().BeNull();
        }
    }
}
