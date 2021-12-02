using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentDetails
{
    public class When_Called_With_Core_Previous_Result_NotAdded : TestSetup
    {
        private IList<AssessmentSeriesDetails> _assessmentSeriesDetails;

        public override void Given()
        {
            expectedApiResult = new LearnerRecord
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = System.DateTime.UtcNow.AddYears(-29),
                Gender = "M",
                Pathway = new Pathway
                {
                    Id = 2,
                    LarId = "89564123",
                    Name = "Test Pathway",
                    Title = "Test Pathwya title",
                    AcademicYear = 2020,
                    Status = RegistrationPathwayStatus.Active,
                    Provider = new Provider
                    {
                        Id = 1,
                        Ukprn = 456123987,
                        Name = "Provider Name",
                        DisplayName = "Provider display name",
                    },
                    PathwayAssessments = new List<Assessment>
                    {
                        new Assessment
                        {
                            Id = 3,
                            SeriesId = 1,
                            SeriesName = "Summer 2021",
                            AppealEndDate = System.DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = System.DateTime.UtcNow,
                            Results = new List<Result>() // No result                            
                        }
                    }
                }
            };

            _assessmentSeriesDetails = new List<AssessmentSeriesDetails>
            {
                new AssessmentSeriesDetails
                {
                    Id = 1,
                    ComponentType = ComponentType.Core,
                    Name = "Summer 2021",
                    Description = "Summer 2021",
                    StartDate = System.DateTime.UtcNow.AddDays(-10),
                    EndDate = System.DateTime.UtcNow.AddDays(-1),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(10),
                    Year = 2021
                },
                new AssessmentSeriesDetails
                {
                    Id = 2,
                    ComponentType = ComponentType.Core,
                    Name = "Summer 2022",
                    Description = "Summer 2022",
                    StartDate = System.DateTime.UtcNow.AddDays(-1),
                    EndDate = System.DateTime.UtcNow.AddDays(30),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(40),
                    Year = 2022
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
            InternalApiClient.GetAssessmentSeriesAsync().Returns(_assessmentSeriesDetails);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var previousCoreAssessmentSeriesId = _assessmentSeriesDetails[0].Id;
            var coreAssessmentSeriesName = _assessmentSeriesDetails[1].Name;
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.Pathway.Provider.Ukprn);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.Pathway.Title);
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Pathway.Status);

            ActualResult.IsCoreEntryEligible.Should().BeTrue();
            ActualResult.PathwayId.Should().Be(expectedApiResult.Pathway.Id);
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResult.Pathway.Name} ({expectedApiResult.Pathway.LarId})");
            ActualResult.NextAvailableCoreSeries.Should().Be(coreAssessmentSeriesName);

            ActualResult.PathwayAssessment.Should().BeNull();
            ActualResult.PreviousPathwayAssessment.Should().NotBeNull();

            var expectedPreviousPathwayAssessment = expectedApiResult.Pathway.PathwayAssessments.FirstOrDefault(p => p.SeriesId == previousCoreAssessmentSeriesId);
            ActualResult.PreviousPathwayAssessment.AssessmentId.Should().Be(expectedPreviousPathwayAssessment.Id);
            ActualResult.PreviousPathwayAssessment.SeriesId.Should().Be(expectedPreviousPathwayAssessment.SeriesId);
            ActualResult.PreviousPathwayAssessment.SeriesName.Should().Be(expectedPreviousPathwayAssessment.SeriesName);
            ActualResult.PreviousPathwayAssessment.LastUpdatedOn.Should().Be(expectedPreviousPathwayAssessment.LastUpdatedOn);
            ActualResult.PreviousPathwayAssessment.LastUpdatedBy.Should().Be(expectedPreviousPathwayAssessment.LastUpdatedBy);

            ActualResult.SpecialismDetails.Should().BeNullOrEmpty();
            ActualResult.IsSpecialismEntryEligible.Should().BeFalse();
            ActualResult.HasCurrentSpecialismAssessmentEntry.Should().BeFalse();
            ActualResult.IsResitForSpecialism.Should().BeFalse();
            ActualResult.NextAvailableSpecialismSeries.Should().BeNullOrEmpty();
            
            ActualResult.IsCoreResultExist.Should().BeFalse();
            ActualResult.HasAnyOutstandingPathwayPrsActivities.Should().BeFalse();
            ActualResult.IsIndustryPlacementExist.Should().BeFalse();

            ActualResult.HasCurrentCoreAssessmentEntry.Should().BeFalse();
            ActualResult.HasResultForCurrentCoreAssessment.Should().BeFalse();
            ActualResult.HasPreviousCoreAssessment.Should().BeTrue();
            ActualResult.HasResultForPreviousCoreAssessment.Should().BeFalse();
            ActualResult.NeedCoreResultForPreviousAssessmentEntry.Should().BeTrue();
            ActualResult.DisplayMultipleSpecialismsCombined.Should().BeFalse();
            ActualResult.IsSpecialismRegistered.Should().BeFalse();
            ActualResult.CombinedSpecialismDisplayName.Should().BeNull();
            ActualResult.DisplaySpecialisms.Should().BeNullOrEmpty();
        }
    }
}
