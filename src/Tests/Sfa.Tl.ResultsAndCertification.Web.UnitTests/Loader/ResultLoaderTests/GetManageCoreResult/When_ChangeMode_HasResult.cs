using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageCoreResult
{
    public class When_ChangeMode_HasResult : TestSetup
    {
        public override void Given()
        {
            IsChangeMode = true;

            expectedApiLookupData = new List<LookupData>
            {
                new LookupData { Id = 1, Code = "C1", Value = "V1" },
                new LookupData { Id = 2, Code = "C2", Value = "V2" }
            };

            InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(expectedApiLookupData);

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
                            Results = new List<Result>
                            {
                                new Result
                                {
                                    Id = 1,
                                    Grade = "A",
                                    PrsStatus = null,
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow
                                }
                            }
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
                                    Id = 2,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = DateTime.UtcNow,
                                    Results = new List<Result>()
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


            var expectedPathwayAsssessment = expectedApiResultDetails.Pathway.PathwayAssessments.FirstOrDefault();
            ActualResult.AssessmentId.Should().Be(expectedPathwayAsssessment.Id);
            ActualResult.AssessmentSeries.Should().Be(expectedPathwayAsssessment.SeriesName.ToLowerInvariant());
            ActualResult.AppealEndDate.Should().Be(expectedPathwayAsssessment.AppealEndDate);

            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResultDetails.Pathway.Name} ({expectedApiResultDetails.Pathway.LarId})");

            var expectedResult = expectedPathwayAsssessment.Results.FirstOrDefault();
            ActualResult.ResultId.Should().Be(expectedResult.Id);
            ActualResult.SelectedGradeCode.Should().Be(expectedResult.Grade);
            ActualResult.PathwayPrsStatus.Should().Be(expectedResult.PrsStatus);
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
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }
    }
}
