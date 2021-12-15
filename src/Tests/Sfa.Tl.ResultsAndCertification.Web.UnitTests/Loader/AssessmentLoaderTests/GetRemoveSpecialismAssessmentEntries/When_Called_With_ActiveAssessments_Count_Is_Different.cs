using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetRemoveSpecialismAssessmentEntries
{
    public class When_Called_With_ActiveAssessments_Count_Is_Different : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;
        private IList<AssessmentEntryDetails> _expectedApiAssessmentEntryDetails;

        public override void Given()
        {
            _expectedApiLearnerResult = new LearnerRecord
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
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 5,
                            LarId = "ZT2158963",
                            Name = "Specialism 1",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 4,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = System.DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow
                                }
                            }
                        },
                        new Specialism
                        {
                            Id = 6,
                            LarId = "ZT2158999",
                            Name = "Specialism 2",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 5,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = System.DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow
                                }
                            }
                        }
                    },
                }
            };

            _expectedApiAssessmentEntryDetails = new List<AssessmentEntryDetails>
            {
                new AssessmentEntryDetails { Uln = 1234567890, ProfileId = 1, AssessmentId = 4, AssessmentSeriesId = 1, AssessmentSeriesName = "Autumn 2021" }
            };

            SpecialismAssessmentIds = string.Join(Constants.PipeSeperator, _expectedApiLearnerResult.Pathway.Specialisms.SelectMany(x => x.Assessments).Select(x => x.Id));
            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(_expectedApiLearnerResult);
            InternalApiClient.GetActiveSpecialismAssessmentEntriesAsync(AoUkprn, SpecialismAssessmentIds)
                .Returns(_expectedApiAssessmentEntryDetails);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            ActualResult.Should().BeNull();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId);
            InternalApiClient.Received(1).GetActiveSpecialismAssessmentEntriesAsync(AoUkprn, SpecialismAssessmentIds);
        }
    }
}
