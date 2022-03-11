using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_PrsLearnerDetailsViewModel : TestSetup
    {
        private LearnerRecord _expectedApiResult;

        protected PrsLearnerDetailsViewModel1 ActualResult { get; set; }

        public override void Given()
        {
            ProfileId = 1;
            _expectedApiResult = new LearnerRecord
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-29),
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
                            Id = 11,
                            SeriesId = 1,
                            SeriesName = "Autumn 2022",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow
                        },

                        new Assessment
                        {
                            Id = 12,
                            SeriesId = 2,
                            SeriesName = "Summer 2022",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "C",
                                PrsStatus = null,
                                LastUpdatedBy = "System",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        },

                        new Assessment
                        {
                            Id = 13,
                            SeriesId = 3,
                            SeriesName = "Autumn 2021",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "B",
                                PrsStatus = PrsStatus.BeingAppealed,
                                LastUpdatedBy = "System",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        },

                        new Assessment
                        {
                            Id = 14,
                            SeriesId = 4,
                            SeriesName = "Summer 2021",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "A",
                                PrsStatus = PrsStatus.Final,
                                LastUpdatedBy = "System",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        },
                        new Assessment
                        {
                            Id = 15,
                            SeriesId = 5,
                            SeriesName = "Autumn 2020",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "D",
                                PrsStatus = PrsStatus.UnderReview,
                                LastUpdatedBy = "System",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        },
                        new Assessment
                        {
                            Id = 16,
                            SeriesId = 5,
                            SeriesName = "Autumn 2020",
                            ComponentType = ComponentType.Core,
                            RommEndDate = DateTime.UtcNow.AddDays(-15),
                            AppealEndDate = DateTime.UtcNow.AddDays(-10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "D",
                                PrsStatus = null,
                                LastUpdatedBy = "System",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        }
                    },
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 20,
                            LarId = "12345678",
                            Name = "Plumbing",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 100,
                                    SeriesId = 1,
                                    SeriesName = "Summer 2022",
                                    ComponentType = ComponentType.Specialism,
                                    RommEndDate = DateTime.UtcNow.AddDays(15),
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                },
                                new Assessment
                                {
                                    Id = 101,
                                    SeriesId = 2,
                                    SeriesName = "Summer 2021",
                                    ComponentType = ComponentType.Specialism,
                                    RommEndDate = DateTime.UtcNow.AddDays(15),
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                    Result = new Result
                                    {
                                        Id = 1,
                                        Grade = "Merit",
                                        PrsStatus = null,
                                        LastUpdatedBy = "System",
                                        LastUpdatedOn = DateTime.UtcNow
                                    }
                                }
                            }
                        },

                        new Specialism
                        {
                            Id = 22,
                            LarId = "12345679",
                            Name = "Heating",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 102,
                                    SeriesId = 2,
                                    SeriesName = "Summer 2021",
                                    ComponentType = ComponentType.Specialism,
                                    RommEndDate = DateTime.UtcNow.AddDays(-40),
                                    AppealEndDate = DateTime.UtcNow.AddDays(-30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                    Result = new Result
                                    {
                                        Id = 1,
                                        Grade = "Pass",
                                        PrsStatus = null,
                                        LastUpdatedBy = "System",
                                        LastUpdatedOn = DateTime.UtcNow
                                    }
                                }
                            }
                        },
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

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.Pathway.Title);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.Pathway.Provider.Ukprn);

            // Core Component 
            ActualResult.HasCoreResults.Should().BeTrue();
            ActualResult.CoreComponentDisplayName.Should().Be($"{_expectedApiResult.Pathway.Name} ({_expectedApiResult.Pathway.LarId})");
            ActualResult.PrsCoreComponentExams.Should().HaveCount(_expectedApiResult.Pathway.PathwayAssessments.Count(p => p.Result != null));
            
            foreach (var expectedExam in _expectedApiResult.Pathway.PathwayAssessments.Where(p => p.Result != null))
            {
                var actualExam = ActualResult.PrsCoreComponentExams.FirstOrDefault(x => x.AssessmentId == expectedExam.Id);
                actualExam.Should().NotBeNull();
                actualExam.AssessmentSeries.Should().Be(expectedExam.SeriesName);
                actualExam.RommEndDate.Should().Be(expectedExam.RommEndDate);
                actualExam.AppealEndDate.Should().Be(expectedExam.AppealEndDate);

                var isResultAvailable = expectedExam.Result != null;
                var isGradeExists = expectedExam.Id > 0 && isResultAvailable && !string.IsNullOrWhiteSpace(expectedExam.Result.Grade);
                var isAddRommAllowed = isGradeExists && (expectedExam.Result.PrsStatus == null || expectedExam.Result.PrsStatus == PrsStatus.NotSpecified) && (DateTime.UtcNow <= expectedExam.RommEndDate);
                var isAddRommOutcomeAllowed = expectedExam.Result?.PrsStatus == PrsStatus.UnderReview;

                actualExam.Grade.Should().Be(!isResultAvailable ? null : expectedExam.Result.Grade);
                actualExam.PrsStatus.Should().Be(!isResultAvailable ? null : expectedExam.Result.PrsStatus);
                actualExam.LastUpdated.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedOn.ToDobFormat());
                actualExam.UpdatedBy.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedBy);
                actualExam.ComponentType.Should().Be(ComponentType.Core);
                actualExam.IsAddRommAllowed.Should().Be(isAddRommAllowed);
                actualExam.IsAddRommOutcomeAllowed.Should().Be(isAddRommOutcomeAllowed);
                actualExam.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            }

            // Specialism Components
            ActualResult.PrsSpecialismComponents.Should().HaveCount(_expectedApiResult.Pathway.Specialisms.Count());

            foreach (var expectedSpecialism in _expectedApiResult.Pathway.Specialisms)
            {
                var actualSpecialism = ActualResult.PrsSpecialismComponents.FirstOrDefault(x => x.SpecialismComponentDisplayName.Equals($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})", StringComparison.InvariantCultureIgnoreCase));
                actualSpecialism.Should().NotBeNull();

                foreach (var expectedExam in expectedSpecialism.Assessments.Where(s => s.Result != null))
                {
                    var actualExam = actualSpecialism.SpecialismComponentExams.FirstOrDefault(x => x.AssessmentId == expectedExam.Id);
                    actualExam.Should().NotBeNull();
                    actualExam.AssessmentSeries.Should().Be(expectedExam.SeriesName);
                    actualExam.RommEndDate.Should().Be(expectedExam.RommEndDate);
                    actualExam.AppealEndDate.Should().Be(expectedExam.AppealEndDate);

                    var isResultAvailable = expectedExam.Result != null;
                    var isGradeExists = expectedExam.Id > 0 && isResultAvailable && !string.IsNullOrWhiteSpace(expectedExam.Result.Grade);
                    var isAddRommAllowed = isGradeExists && (expectedExam.Result.PrsStatus == null || expectedExam.Result.PrsStatus == PrsStatus.NotSpecified) && (DateTime.UtcNow <= expectedExam.RommEndDate);
                    var isAddRommOutcomeAllowed = expectedExam.Result?.PrsStatus == PrsStatus.UnderReview;

                    actualExam.Grade.Should().Be(!isResultAvailable ? null : expectedExam.Result.Grade);
                    actualExam.PrsStatus.Should().Be(!isResultAvailable ? null : expectedExam.Result.PrsStatus);
                    actualExam.LastUpdated.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedOn.ToDobFormat());
                    actualExam.UpdatedBy.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedBy);
                    actualExam.ComponentType.Should().Be(ComponentType.Specialism);
                    actualExam.IsAddRommAllowed.Should().Be(isAddRommAllowed);
                    actualExam.IsAddRommOutcomeAllowed.Should().Be(isAddRommOutcomeAllowed);
                    actualExam.ProfileId.Should().Be(_expectedApiResult.ProfileId);
                }
            }
        }
    }
}