﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new LearnerRecord
            {
                ProfileId = 1,
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
                            ResultEndDate = DateTime.UtcNow.AddDays(1),
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
                            ResultEndDate = DateTime.UtcNow.AddDays(-1),
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
                            ResultEndDate = DateTime.UtcNow.AddDays(1),
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
                            ResultEndDate = DateTime.UtcNow.AddDays(1),
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
                            ResultEndDate = DateTime.UtcNow.AddDays(-20),
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
                                    ResultEndDate = DateTime.UtcNow.AddDays(-1),
                                    RommEndDate = DateTime.UtcNow.AddDays(5),
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
                                    ResultEndDate = DateTime.UtcNow.AddDays(1),
                                    RommEndDate = DateTime.UtcNow.AddDays(5),
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                    Result = new Result
                                    {
                                        Id = 1,
                                        Grade = "Merit",
                                        PrsStatus = PrsStatus.Reviewed,
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
                                    ResultEndDate = DateTime.UtcNow.AddDays(-40),
                                    RommEndDate = DateTime.UtcNow.AddDays(-35),
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

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, CoreStatus).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId, CoreStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            
            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.Pathway.Title);
            ActualResult.ProviderName.Should().Be(expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.Pathway.Provider.Ukprn);
            
            // Core Component 
            ActualResult.IsCoreAssessmentEntryRegistered.Should().BeTrue();
            ActualResult.CoreComponentDisplayName.Should().Be($"{expectedApiResult.Pathway.Name} ({expectedApiResult.Pathway.LarId})");
            ActualResult.CoreComponentExams.Should().HaveCount(expectedApiResult.Pathway.PathwayAssessments.Count());
            foreach (var expectedExam in expectedApiResult.Pathway.PathwayAssessments)
            {
                var actualExam = ActualResult.CoreComponentExams.FirstOrDefault(x => x.AssessmentId == expectedExam.Id);
                actualExam.Should().NotBeNull();
                actualExam.AssessmentSeries.Should().Be(expectedExam.SeriesName);
                actualExam.ResultEndDate.Should().Be(expectedExam.ResultEndDate);
                actualExam.RommEndDate.Should().Be(expectedExam.RommEndDate);
                actualExam.AppealEndDate.Should().Be(expectedExam.AppealEndDate);               

                var isResultAvailable = expectedExam.Result != null;
                actualExam.Grade.Should().Be(!isResultAvailable ? null : expectedExam.Result.Grade);
                actualExam.PrsStatus.Should().Be(!isResultAvailable ? null : expectedExam.Result.PrsStatus);
                actualExam.LastUpdated.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedOn.ToDobFormat());
                actualExam.UpdatedBy.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedBy);
                actualExam.ComponentType.Should().Be(ComponentType.Core);
                actualExam.ProfileId.Should().Be(expectedApiResult.ProfileId);
                                
                var isResultChangeAllowed = isResultAvailable && (expectedExam.Result.PrsStatus == null || expectedExam.Result.PrsStatus == PrsStatus.NotSpecified) && DateTime.Today <= expectedExam.ResultEndDate;
                actualExam.IsResultChangeAllowed.Should().Be(isResultChangeAllowed);
            }

            // Specialism Components
            ActualResult.SpecialismComponents.Should().HaveCount(expectedApiResult.Pathway.Specialisms.Count());
            foreach (var expectedSpecialism in expectedApiResult.Pathway.Specialisms)
            {
                var actualSpecialism = ActualResult.SpecialismComponents.FirstOrDefault(x => x.SpecialismComponentDisplayName.Equals($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})", StringComparison.InvariantCultureIgnoreCase));
                actualSpecialism.Should().NotBeNull();
                actualSpecialism.LarId.Should().Be(expectedSpecialism.LarId);
                actualSpecialism.IsCouplet.Should().BeFalse();

                foreach (var expectedExam in expectedSpecialism.Assessments)
                {
                    var actualExam = actualSpecialism.SpecialismComponentExams.FirstOrDefault(x => x.AssessmentId == expectedExam.Id);
                    actualExam.Should().NotBeNull();
                    actualExam.AssessmentSeries.Should().Be(expectedExam.SeriesName);
                    actualExam.ResultEndDate.Should().Be(expectedExam.ResultEndDate);
                    actualExam.RommEndDate.Should().Be(expectedExam.RommEndDate);
                    actualExam.AppealEndDate.Should().Be(expectedExam.AppealEndDate);

                    var isResultAvailable = expectedExam.Result != null;
                    actualExam.Grade.Should().Be(!isResultAvailable ? null : expectedExam.Result.Grade);
                    actualExam.PrsStatus.Should().Be(!isResultAvailable ? null : expectedExam.Result.PrsStatus);
                    actualExam.LastUpdated.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedOn.ToDobFormat());
                    actualExam.UpdatedBy.Should().Be(!isResultAvailable ? null : expectedExam.Result.LastUpdatedBy);
                    actualExam.ComponentType.Should().Be(ComponentType.Specialism);
                    actualExam.ProfileId.Should().Be(expectedApiResult.ProfileId);

                    var isResultChangeAllowed = isResultAvailable && (expectedExam.Result.PrsStatus == null || expectedExam.Result.PrsStatus == PrsStatus.NotSpecified) && DateTime.Today <= expectedExam.ResultEndDate;
                    actualExam.IsResultChangeAllowed.Should().Be(isResultChangeAllowed);
                }
            }
        }
    }
}