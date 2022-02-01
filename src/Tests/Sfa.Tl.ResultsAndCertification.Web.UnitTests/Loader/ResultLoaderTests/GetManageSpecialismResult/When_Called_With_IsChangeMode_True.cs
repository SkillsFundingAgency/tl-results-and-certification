using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageSpecialismResult
{
    public class When_Called_With_IsChangeMode_True : TestSetup
    {
        public override void Given()
        {
            IsChangeMode = true;

            expectedApiLookupData = new List<LookupData>
            {
                new LookupData { Id = 1, Code = "C1", Value = "V1" },
                new LookupData { Id = 2, Code = "C2", Value = "V2" },
                new LookupData { Code = "NR", Value = "This learner's grade has not been received" },
            };

            InternalApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(expectedApiLookupData);

            expectedApiResultDetails = new LearnerRecord
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
                            Id = AssessmentId,
                            SeriesId = 1,
                            SeriesName = "Summer 2021",
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = null
                        }
                    },
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 1,
                            LarId = "ZT2158963",
                            Name = "Specialism Name",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = AssessmentId,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                    Result = new Result
                                    {
                                        Id = 1,
                                        Grade = "Pass",
                                        GradeCode = "C1",
                                        PrsStatus = null
                                    }
                                }
                            }
                        }
                    }
                }
            };
            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(expectedApiResultDetails.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResultDetails.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResultDetails.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResultDetails.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResultDetails.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResultDetails.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResultDetails.Pathway.Provider.Ukprn);
            ActualResult.TlevelTitle.Should().Be(expectedApiResultDetails.Pathway.Title);

            var expectedSpecialism = expectedApiResultDetails.Pathway.Specialisms.FirstOrDefault(s => s.Assessments.Any(a => a.Id == AssessmentId));
            var expectedSpecialismAsssessment = expectedSpecialism.Assessments.FirstOrDefault(sa => sa.Id == AssessmentId);
            ActualResult.AssessmentId.Should().Be(expectedSpecialismAsssessment.Id);
            ActualResult.AssessmentSeries.Should().Be(expectedSpecialismAsssessment.SeriesName.ToLowerInvariant());
            ActualResult.AppealEndDate.Should().Be(expectedSpecialismAsssessment.AppealEndDate);

            var expectedResult = expectedSpecialismAsssessment.Result;
            ActualResult.SpecialismName.Should().Be(expectedSpecialism.Name);
            ActualResult.SpecialismDisplayName.Should().Be($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})");
            ActualResult.ResultId.Should().Be(expectedResult.Id);
            ActualResult.SelectedGradeCode.Should().Be(expectedResult.GradeCode);
            ActualResult.PrsStatus.Should().BeNull();

            ActualResult.IsValid.Should().BeTrue();

            ActualResult.Grades.Should().NotBeNull();
            ActualResult.Grades.Count.Should().Be(expectedApiLookupData.Count);

            for (int i = 0; i < ActualResult.Grades.Count; i++)
            {
                ActualResult.Grades[i].Id.Should().Be(expectedApiLookupData[i].Id);
                ActualResult.Grades[i].Code.Should().Be(expectedApiLookupData[i].Code);
                ActualResult.Grades[i].Value.Should().Be(expectedApiLookupData[i].Value);
            }
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }
    }
}
