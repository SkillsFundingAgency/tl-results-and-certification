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
    public class When_Called_With_Valid_Data : TestSetup
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
                            Results = new List<Result>
                            {
                                new Result
                                {
                                    Id = 1,
                                    Grade = "A",
                                    PrsStatus = null,
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow
                                }
                            }
                        }
                    },
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 5,
                            LarId = "ZT2158963",
                            Name = "Specialism Name",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 4,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = System.DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow,
                                    Results = new List<Result>()
                                }
                            }
                        }
                    },
                    IndustryPlacements = new List<IndustryPlacement>
                    {
                        new IndustryPlacement
                        {
                            Id = 7,
                            Status = IndustryPlacementStatus.Completed
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
                    StartDate = System.DateTime.UtcNow.AddDays(-1),
                    EndDate = System.DateTime.UtcNow.AddDays(10),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(20),
                    Year = 2021
                },
                new AssessmentSeriesDetails
                {
                    Id = 2,
                    ComponentType = ComponentType.Specialism,
                    Name = "Summer 2022",
                    Description = "Summer 2022",
                    StartDate = System.DateTime.UtcNow.AddDays(-1),
                    EndDate = System.DateTime.UtcNow.AddDays(10),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(20),
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
            ActualResult.Should().NotBeNull();

            var coreAssessmentSeriesId = _assessmentSeriesDetails[0].Id;
            var coreAssessmentSeriesName = _assessmentSeriesDetails[0].Name;

            var specialismAssessmentSeriesId = _assessmentSeriesDetails[1].Id;
            var specialismAssessmentSeriesName = _assessmentSeriesDetails[1].Name;

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.Pathway.Provider.Ukprn);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.Pathway.Title);
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Pathway.Status);
                        
            ActualResult.IsCoreEntryEligible.Should().Be(expectedApiResult.Pathway.Status == RegistrationPathwayStatus.Active && coreAssessmentSeriesId > 0);
            ActualResult.PathwayId.Should().Be(expectedApiResult.Pathway.Id);
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResult.Pathway.Name} ({expectedApiResult.Pathway.LarId})");
            ActualResult.NextAvailableCoreSeries.Should().Be(coreAssessmentSeriesName);

            var expectedPathwayAssessment = expectedApiResult.Pathway.PathwayAssessments.FirstOrDefault(p => p.SeriesId == coreAssessmentSeriesId);

            ActualResult.PathwayAssessment.AssessmentId.Should().Be(expectedPathwayAssessment.Id);
            ActualResult.PathwayAssessment.SeriesId.Should().Be(expectedPathwayAssessment.SeriesId);
            ActualResult.PathwayAssessment.SeriesName.Should().Be(expectedPathwayAssessment.SeriesName);
            ActualResult.PathwayAssessment.LastUpdatedOn.Should().Be(expectedPathwayAssessment.LastUpdatedOn);
            ActualResult.PathwayAssessment.LastUpdatedBy.Should().Be(expectedPathwayAssessment.LastUpdatedBy);

            ActualResult.PathwayAssessment.Results.Count().Should().Be(expectedPathwayAssessment.Results.Count());
           
            foreach(var expectedResult in expectedPathwayAssessment.Results)
            {
                var actualResult = ActualResult.PathwayAssessment.Results.FirstOrDefault(r => r.Id == expectedResult.Id);
                actualResult.Should().NotBeNull();

                actualResult.Id.Should().Be(expectedResult.Id);
                actualResult.Grade.Should().Be(expectedResult.Grade);
                actualResult.LastUpdatedBy.Should().Be(expectedResult.LastUpdatedBy);
                actualResult.LastUpdatedOn.Should().Be(expectedResult.LastUpdatedOn);
            }

            ActualResult.PreviousPathwayAssessment.Should().BeNull();

            foreach (var expectedSpecialism in expectedApiResult.Pathway.Specialisms)
            {
                var actualSpecialism = ActualResult.SpecialismDetails.FirstOrDefault(s => s.Id == expectedSpecialism.Id);

                actualSpecialism.Should().NotBeNull();

                actualSpecialism.Id.Should().Be(expectedSpecialism.Id);
                actualSpecialism.LarId.Should().Be(expectedSpecialism.LarId);
                actualSpecialism.Name.Should().Be(expectedSpecialism.Name);
                actualSpecialism.DisplayName.Should().Be($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})");

                foreach (var expectedSpecialismAssessment in expectedSpecialism.Assessments)
                {
                    var actualSpecialismAssessment = actualSpecialism.Assessments.FirstOrDefault(r => r.AssessmentId == expectedSpecialismAssessment.Id);
                    actualSpecialismAssessment.Should().NotBeNull();

                    actualSpecialismAssessment.AssessmentId.Should().Be(expectedSpecialismAssessment.Id);
                    actualSpecialismAssessment.SeriesId.Should().Be(expectedSpecialismAssessment.SeriesId);
                    actualSpecialismAssessment.SeriesName.Should().Be(expectedSpecialismAssessment.SeriesName);
                    actualSpecialismAssessment.LastUpdatedOn.Should().Be(expectedSpecialismAssessment.LastUpdatedOn);
                    actualSpecialismAssessment.LastUpdatedBy.Should().Be(expectedSpecialismAssessment.LastUpdatedBy);
                }
            }

            ActualResult.IsSpecialismEntryEligible.Should().Be(expectedApiResult.Pathway.Status == RegistrationPathwayStatus.Active && coreAssessmentSeriesId > 0);

            var expectedHasCurrentSpecialismAssessmentEntry = expectedApiResult.Pathway.Specialisms.SelectMany(sa => sa.Assessments).Any(a => a.SeriesId == specialismAssessmentSeriesId);
            ActualResult.HasCurrentSpecialismAssessmentEntry.Should().Be(expectedHasCurrentSpecialismAssessmentEntry);

            var expectedIsResitForSpecialism = expectedApiResult.Pathway.Specialisms.SelectMany(sa => sa.Assessments).Any(a => a.SeriesId != specialismAssessmentSeriesId);
            ActualResult.IsResitForSpecialism.Should().Be(expectedIsResitForSpecialism);
            ActualResult.NextAvailableSpecialismSeries.Should().Be(specialismAssessmentSeriesName);

            var expectedIsCoreResultExists = expectedApiResult.Pathway.PathwayAssessments.Any(a => a.Results.Any());
            ActualResult.IsCoreResultExist.Should().Be(expectedIsCoreResultExists);

            var expectedHasAnyOutstandingPathwayPrsActivities = expectedApiResult.Pathway.PathwayAssessments.Any(a => a.Results.Any(r => r.PrsStatus == PrsStatus.BeingAppealed));
            ActualResult.HasAnyOutstandingPathwayPrsActivities.Should().Be(expectedHasAnyOutstandingPathwayPrsActivities);

            var expectedIsIndustryPlacementExist = expectedApiResult.Pathway.IndustryPlacements.Any();
            ActualResult.IsIndustryPlacementExist.Should().Be(expectedIsIndustryPlacementExist);


            ActualResult.HasCurrentCoreAssessmentEntry.Should().Be(true);
            ActualResult.HasResultForCurrentCoreAssessment.Should().Be(true);
            ActualResult.HasPreviousCoreAssessment.Should().Be(false);
            ActualResult.HasResultForPreviousCoreAssessment.Should().Be(false);
            ActualResult.NeedCoreResultForPreviousAssessmentEntry.Should().Be(false);
            ActualResult.DisplayMultipleSpecialismsCombined.Should().Be(false);
            ActualResult.IsSpecialismRegistered.Should().Be(true);
            ActualResult.SpecialismDisplayName.Should().BeNull();

            var expectedDisplaySpecialisms = new List<SpecialismViewModel>();
            foreach (var specialism in expectedApiResult.Pathway.Specialisms)
            {
                expectedDisplaySpecialisms.Add(new SpecialismViewModel
                {
                    Id = specialism.Id,
                    LarId = specialism.LarId,
                    Name = specialism.Name,
                    DisplayName = $"{specialism.Name} ({specialism.LarId})"
                });
            }
            ActualResult.DisplaySpecialisms.Count().Should().Be(expectedDisplaySpecialisms.Count());

            foreach(var expectedDisplaySpecialism in expectedDisplaySpecialisms)
            {
                var actualDisplaySpecialism = ActualResult.DisplaySpecialisms.FirstOrDefault(s => s.Id == expectedDisplaySpecialism.Id);

                actualDisplaySpecialism.Should().NotBeNull();
                actualDisplaySpecialism.Id.Should().Be(expectedDisplaySpecialism.Id);
                actualDisplaySpecialism.LarId.Should().Be(expectedDisplaySpecialism.LarId);
                actualDisplaySpecialism.Name.Should().Be(expectedDisplaySpecialism.Name);
                actualDisplaySpecialism.DisplayName.Should().Be($"{expectedDisplaySpecialism.Name} ({expectedDisplaySpecialism.LarId})");
            }
        }
    }
}
