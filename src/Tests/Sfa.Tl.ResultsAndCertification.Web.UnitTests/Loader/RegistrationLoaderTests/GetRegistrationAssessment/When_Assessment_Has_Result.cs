using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationAssessment
{
    public class When_Assessment_Has_Result : TestSetup
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

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_IsResultExist_IsTrue()
        {
            ActualResult.IsCoreResultExist.Should().BeTrue();
        }
    }
}
