﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_PrsRommGradeChangeViewModel_For_Specialism : TestSetup
    {
        private LearnerRecord _expectedApiResult;
        private List<LookupData> _apiGrades;

        protected PrsRommGradeChangeViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 101;

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
                            RommEndDate = DateTime.UtcNow.AddDays(5),
                            AppealEndDate = DateTime.UtcNow.AddDays(10),
                            ComponentType = ComponentType.Core,
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
                                        PrsStatus = null,
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
            _apiGrades = new List<LookupData> {
                new LookupData { Id = 1, Code = "SCG1", Value = "Distinction" },
                new LookupData { Id = 2, Code = "SCG2", Value = "Merit" },
                new LookupData { Id = 3, Code = "SCG3", Value = "Pass" },
                new LookupData { Id = 4, Code = "SCG5", Value = "Q - pending result" },
                new LookupData { Id = 5, Code = "SCG6", Value = "X - no result" }
            };
            InternalApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(_apiGrades);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Specialism);
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

            var expectedSpecialism = _expectedApiResult.Pathway.Specialisms.FirstOrDefault(s => s.Assessments.Any(a => a.Id == AssessmentId));
            var expectedAssessment = expectedSpecialism?.Assessments?.FirstOrDefault(sa => sa.Id == AssessmentId);
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.AssessmentId.Should().Be(expectedAssessment.Id);
            ActualResult.RommEndDate.Should().Be(expectedAssessment.RommEndDate);
            ActualResult.PrsStatus.Should().Be(expectedAssessment.Result.PrsStatus);
            ActualResult.ComponentType.Should().Be(ComponentType.Specialism);

            // Specialism Component 
            ActualResult.SpecialismDisplayName.Should().Be($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})");
            ActualResult.ExamPeriod.Should().Be(expectedAssessment.SeriesName);
            ActualResult.Grade.Should().Be(expectedAssessment.Result.Grade);
            ActualResult.GradeCode.Should().Be(expectedAssessment.Result.GradeCode);

            // Assert Gardes
            ActualResult.Grades.Should().HaveCount(_apiGrades.Count - 1);
            ActualResult.Grades.Select(x => x.Code).Should().NotContain(Constants.SpecialismComponentGradeQpendingResultCode);
        }
    }
}
