﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_Multiple_Specialism_No_Resit_Assessment_Entry : TestSetup
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
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        CurrentSpecialismAssessmentSeriesId = 1,
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>
                        {
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 1,
                                SeriesId = 1,
                                SeriesName = "Summer 2022",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow
                            }
                        }
                    },
                    new SpecialismViewModel
                    {
                        Id = 6,
                        LarId = "ZT2158999",
                        Name = "Specialism Name2",
                        DisplayName = "Specialism Name2 (ZT2158999)",
                        CurrentSpecialismAssessmentSeriesId = 1,
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "ZT2158963|ZT2158999") },
                        Assessments = new List<SpecialismAssessmentViewModel>
                        {
                            new SpecialismAssessmentViewModel
                            {
                                AssessmentId = 2,
                                SeriesId = 1,
                                SeriesName = "Summer 2022",
                                LastUpdatedBy = "Test user",
                                LastUpdatedOn = DateTime.UtcNow
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

            // Specialism DisplayName
            model.DisplaySpecialisms.Should().NotBeNullOrEmpty();
            model.DisplaySpecialisms.Count().Should().Be(1);
            
            var expectedSpecialismDisplayName = string.Join(Constants.AndSeperator, _mockresult.SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})"));
            model.DisplaySpecialisms[0].DisplayName.Should().Be(expectedSpecialismDisplayName);

            // Exam Period
            var expectedSpecialism = model.DisplaySpecialisms[0];
            var expectedAssessments = expectedSpecialism.Assessments.ToList();
            
            var examPeriodModel = model.GetSummaryExamPeriod(expectedSpecialism);
            examPeriodModel.Title.Should().Be(AssessmentDetailsContent.Title_Exam_Period);
            examPeriodModel.Value.Should().Be(expectedAssessments[0].SeriesName);
            examPeriodModel.ActionText.Should().Be(AssessmentDetailsContent.Remove_Action_Link_Text);
            examPeriodModel.HiddenActionText.Should().Be(AssessmentDetailsContent.Remove_Action_Link_Hidden_Text);
            examPeriodModel.ActionText.Should().Be(AssessmentDetailsContent.Remove_Action_Link_Text);
            examPeriodModel.RouteName.Should().Be(RouteConstants.RemoveSpecialismAssessmentEntries);
            examPeriodModel.RouteAttributes.Should().NotBeNull();
            examPeriodModel.RouteAttributes.Count.Should().Be(2);
            examPeriodModel.RouteAttributes[Constants.ProfileId].Should().Be(_mockresult.ProfileId.ToString());
            examPeriodModel.RouteAttributes[Constants.SpecialismAssessmentIds].Should().Be(string.Join(Constants.PipeSeperator, _mockresult.SpecialismDetails.SelectMany(x => x.Assessments).Select(a => a.AssessmentId)));

            // Last updated on 
            var summaryLastUpdatedOnModel = model.GetSummaryLastUpdatedOn(expectedSpecialism);

            summaryLastUpdatedOnModel.Title.Should().Be(AssessmentDetailsContent.Title_Last_Updated_On);
            summaryLastUpdatedOnModel.Value.Should().Be(expectedAssessments[0].LastUpdatedOn.ToDobFormat());

            // Last updated by 
            var summaryLastUpdatedByModel = model.GetSummaryLastUpdatedBy(expectedSpecialism);
            summaryLastUpdatedByModel.Title.Should().Be(AssessmentDetailsContent.Title_Last_Updated_By);
            summaryLastUpdatedByModel.Value.Should().Be(expectedAssessments[0].LastUpdatedBy);
        }
    }
}
