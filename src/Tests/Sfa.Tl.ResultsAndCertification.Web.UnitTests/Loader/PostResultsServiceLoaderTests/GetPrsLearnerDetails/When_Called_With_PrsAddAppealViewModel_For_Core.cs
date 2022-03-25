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
    public class When_Called_With_PrsAddAppealViewModel_For_Core : TestSetup
    {
        private LearnerRecord _expectedApiResult;
        protected PrsAddAppealViewModel ActualResult { get; set; }

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
                            Id = 11,
                            SeriesId = 2,
                            SeriesName = "Summer 2022",
                            RommEndDate = DateTime.UtcNow.AddDays(-5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            ComponentType = ComponentType.Core,
                            LastUpdatedBy = "System",
                            LastUpdatedOn = DateTime.UtcNow,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = "C",
                                PrsStatus = PrsStatus.Reviewed,
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
                                    Id = 101,
                                    SeriesId = 2,
                                    SeriesName = "Summer 2021",
                                    RommEndDate = DateTime.UtcNow.AddDays(15),
                                    AppealEndDate = DateTime.UtcNow.AddDays(30),
                                    ComponentType = ComponentType.Specialism,
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
                        }
                    }
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.Pathway.Title);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.Pathway.Provider.Ukprn);
            ActualResult.IsValid.Should().BeTrue();

            var expectedCoreAssessment = _expectedApiResult.Pathway.PathwayAssessments.FirstOrDefault(p => p.Id == AssessmentId);
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.AssessmentId.Should().Be(expectedCoreAssessment.Id);
            ActualResult.AppealEndDate.Should().Be(expectedCoreAssessment.AppealEndDate);
            ActualResult.PrsStatus.Should().Be(expectedCoreAssessment.Result.PrsStatus);
            ActualResult.ComponentType.Should().Be(ComponentType.Core);

            // Core Component 
            ActualResult.CoreDisplayName.Should().Be($"{_expectedApiResult.Pathway.Name} ({_expectedApiResult.Pathway.LarId})");
            ActualResult.ExamPeriod.Should().Be(expectedCoreAssessment.SeriesName);
            ActualResult.Grade.Should().Be(expectedCoreAssessment.Result.Grade);
        }
    }
}
