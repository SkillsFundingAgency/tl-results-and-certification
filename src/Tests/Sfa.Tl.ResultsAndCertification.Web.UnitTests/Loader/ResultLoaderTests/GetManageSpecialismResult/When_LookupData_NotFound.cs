using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageSpecialismResult
{
    public class When_LookupData_NotFound : TestSetup
    {
        public override void Given()
        {
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
                            Id = 1,
                            SeriesId = 1,
                            SeriesName = "Summer 2021",
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Results = new List<Result>()
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
                                    Results = new List<Result>()
                                }
                            }
                        }
                    }
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);

            expectedApiLookupData = new List<LookupData>();
            InternalApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(expectedApiLookupData);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }
    }
}
