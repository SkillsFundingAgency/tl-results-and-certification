using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;
using Xunit;

using SearchLearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SearchLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.SearchLearnerDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PagedResponse<SearchLearnerDetail> _expectedApiResult;

        public override void Given()
        {
            PageNumber = null;

            SearchCriteriaViewModel = new ViewModel.TrainingProvider.Manual.SearchCriteriaViewModel
            {
                AcademicYear = 2020,
                PageNumber = null,
                SearchLearnerFilters = new ViewModel.TrainingProvider.Manual.SearchLearnerFiltersViewModel
                {
                    AcademicYears = new List<FilterLookupData>
                    {
                        new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = true },
                        new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                    },
                    Status = new List<FilterLookupData>
                    {
                        new FilterLookupData { Id = 1, Name = "English", IsSelected = false },
                        new FilterLookupData { Id = 2, Name = "Maths", IsSelected = true }
                    },
                    Tlevels = new List<FilterLookupData>
                    {
                        new FilterLookupData { Id = 1, Name = "Education", IsSelected = true },
                        new FilterLookupData { Id = 2, Name = "Construction", IsSelected = true }
                    },
                    IsApplyFiltersSelected = true
                },
                SearchKey = "Smith",
                IsSearchKeyApplied = true
            };

            _expectedApiResult = new PagedResponse<SearchLearnerDetail>
            {
                TotalRecords = 10,
                Records = new List<SearchLearnerDetail>
                {
                    new SearchLearnerDetail
                    {
                        ProfileId = 1,
                        Uln = 1234567890,
                        Firstname = "John",
                        Lastname = "Smith",
                        TlevelName = "T level in Education and childcare",
                        AcademicYear = 2020,
                        EnglishStatus = SubjectStatus.Achieved,
                        MathsStatus = SubjectStatus.Achieved,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed
                    },
                    new SearchLearnerDetail
                    {
                        ProfileId = 2,
                        Uln = 2222267890,
                        Firstname = "John",
                        Lastname = "Smith2",
                        TlevelName = "T level in Design and Survey",
                        AcademicYear = 2021,
                        EnglishStatus = null,
                        MathsStatus = null,
                        IndustryPlacementStatus = null
                    }
                },
                PagerInfo = new Pager(2, 1, 10)
            };

            InternalApiClient.SearchLearnerDetailsAsync(Arg.Any<SearchLearnerRequest>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.TotalRecords.Should().Be(_expectedApiResult.TotalRecords);
            ActualResult.SearchLearnerDetailsList.Count.Should().Be(2);

            ActualResult.PagerInfo.Should().BeEquivalentTo(_expectedApiResult.PagerInfo);

            ActualResult.SearchLearnerDetailsList[0].ProfileId.Should().Be(_expectedApiResult.Records[0].ProfileId);
            ActualResult.SearchLearnerDetailsList[0].LearnerName.Should().Be(_expectedApiResult.Records[0].Firstname + " " + _expectedApiResult.Records[0].Lastname);
            ActualResult.SearchLearnerDetailsList[0].StartYear.Should().Be(string.Format(SearchLearnerDetailsContent.Start_Year_Value, _expectedApiResult.Records[0].AcademicYear, _expectedApiResult.Records[0].AcademicYear + 1));
            ActualResult.SearchLearnerDetailsList[0].TlevelName.Should().Be(_expectedApiResult.Records[0].TlevelName);
            ActualResult.SearchLearnerDetailsList[0].IsEnglishAdded.Should().BeTrue();
            ActualResult.SearchLearnerDetailsList[0].IsMathsAdded.Should().BeTrue();
            ActualResult.SearchLearnerDetailsList[0].IsIndustryPlacementAdded.Should().BeTrue();

            ActualResult.SearchLearnerDetailsList[1].ProfileId.Should().Be(_expectedApiResult.Records[1].ProfileId);
            ActualResult.SearchLearnerDetailsList[1].LearnerName.Should().Be(_expectedApiResult.Records[1].Firstname + " " + _expectedApiResult.Records[1].Lastname);
            ActualResult.SearchLearnerDetailsList[1].StartYear.Should().Be(string.Format(SearchLearnerDetailsContent.Start_Year_Value, _expectedApiResult.Records[1].AcademicYear, _expectedApiResult.Records[1].AcademicYear + 1));
            ActualResult.SearchLearnerDetailsList[1].TlevelName.Should().Be(_expectedApiResult.Records[1].TlevelName);
            ActualResult.SearchLearnerDetailsList[1].IsEnglishAdded.Should().BeFalse();
            ActualResult.SearchLearnerDetailsList[1].IsMathsAdded.Should().BeFalse();
            ActualResult.SearchLearnerDetailsList[1].IsIndustryPlacementAdded.Should().BeFalse();
        }
    }
}
