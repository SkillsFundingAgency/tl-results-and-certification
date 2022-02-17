using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_Multiple_Specialism_Resit_Assessment_Entry : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                PathwayStatus = RegistrationPathwayStatus.Active,                                
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 7,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        CurrentSpecialismAssessmentSeriesId = 3,
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>
                        {
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 3,
                                SeriesId = 2,
                                SeriesName = "Summer 2023",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow,
                                Result = new ResultViewModel { Id = 12, Grade = "Merit" }
                            },
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 33,
                                SeriesId = 3,
                                SeriesName = "Autumn 2023",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow,
                                Result = new ResultViewModel { Id = 11, Grade = "Merit" }
                            }
                        }
                    },
                    new SpecialismViewModel
                    {
                        Id = 8,
                        LarId = "ZT2158999",
                        Name = "Specialism Name2",
                        DisplayName = "Specialism Name2 (ZT2158999)",
                        CurrentSpecialismAssessmentSeriesId = 3,
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>
                        {
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 4,
                                SeriesId = 2,
                                SeriesName = "Summer 2023",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow,
                                Result = new ResultViewModel { Id = 12, Grade = "Merit" }
                            },
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 44,
                                SeriesId = 3,
                                SeriesName = "Autumn 2023",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow,
                                Result = new ResultViewModel { Id = 12, Grade = "Merit" }
                            }
                        }
                    }
                }
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Assessment_Entry_Details_Shown()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentDetailsViewModel));

            var model = viewResult.Model as AssessmentDetailsViewModel;
            model.Should().NotBeNull();

            foreach (var specialism in _mockresult.SpecialismDetails)
            {
                // Specialism DisplayName
                var actualSpecialism = model.SpecialismDetails.FirstOrDefault(s => s.Id == specialism.Id);
                actualSpecialism.Should().NotBeNull();
                actualSpecialism.DisplayName.Should().Be(specialism.DisplayName);

                var examPeriodModel = model.GetSummaryExamPeriod(actualSpecialism);
                var expectedAssessments = specialism.Assessments.ToList();

                examPeriodModel.Title.Should().Be(AssessmentDetailsContent.Title_Exam_Period);
                examPeriodModel.Value.Should().Be(_mockresult.SpecialismDetails.SelectMany(x => x.Assessments.Where(a => a.SeriesId == x.CurrentSpecialismAssessmentSeriesId)).FirstOrDefault().SeriesName);
                examPeriodModel.ActionText.Should().BeNull();
                examPeriodModel.HiddenActionText.Should().BeNull();
                examPeriodModel.RouteAttributes.Should().BeNull();
            }
        }
    }
}
