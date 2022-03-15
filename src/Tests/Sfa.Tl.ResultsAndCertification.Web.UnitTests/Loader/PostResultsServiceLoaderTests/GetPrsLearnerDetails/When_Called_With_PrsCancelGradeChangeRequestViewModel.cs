using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_PrsCancelGradeChangeRequestViewModel : TestSetup
    {
        private LearnerRecord _expectedApiResult;
        protected PrsCancelGradeChangeRequestViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 11;

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
                            Id = AssessmentId,
                            SeriesId = 2,
                            SeriesName = "Summer 2022",
                            RommEndDate = DateTime.UtcNow.AddDays(-5),
                            AppealEndDate = DateTime.UtcNow.AddDays(-10),
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
                        }
                    }
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            var expectedCoreAssessment = _expectedApiResult.Pathway.PathwayAssessments.FirstOrDefault(p => p.Id == AssessmentId);
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.AssessmentId.Should().Be(expectedCoreAssessment.Id);
            ActualResult.Status.Should().Be(_expectedApiResult.Pathway.Status);
            ActualResult.PrsStatus.Should().Be(expectedCoreAssessment.Result.PrsStatus);

            ActualResult.IsValid.Should().BeTrue();
            ActualResult.AreYouSureToCancel.Should().BeNull();
            ActualResult.IsResultJourney.Should().BeFalse();
        }
    }
}