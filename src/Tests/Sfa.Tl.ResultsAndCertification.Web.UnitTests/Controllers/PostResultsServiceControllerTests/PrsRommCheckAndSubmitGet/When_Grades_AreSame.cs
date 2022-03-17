using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsRommCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommCheckAndSubmitGet
{
    public class When_Grades_AreSame : TestSetup
    {
        private PrsRommCheckAndSubmitViewModel _mockCache = null;

        public override void Given()
        {
            var previousGrade = "A";
            var newGrade = "A";
            ComponentType = ComponentType.Core;

            _mockCache = new PrsRommCheckAndSubmitViewModel
            {
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "Tlevel in Education",
                ProviderName = "Barsley College",
                ProviderUkprn = 87654321,
                CoreDisplayName = "Core Display (1234567)",
                ExamPeriod = "Summer 2021",
                NewGrade = newGrade,
                OldGrade = previousGrade,
                IsGradeChanged = false,
                ComponentType = ComponentType,

                ProfileId = 1,
                AssessmentId = 2
            };
            CacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsRommCheckAndSubmitViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.Firstname.Should().Be(_mockCache.Firstname);
            model.Lastname.Should().Be(_mockCache.Lastname);
            model.LearnerName.Should().Be($"{_mockCache.Firstname} {_mockCache.Lastname}");
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.ProviderUkprn.Should().Be(_mockCache.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockCache.TlevelTitle);
            model.CoreDisplayName.Should().Be(_mockCache.CoreDisplayName);
            model.NewGrade.Should().Be(_mockCache.NewGrade);
            model.OldGrade.Should().Be(_mockCache.OldGrade);
            model.ComponentType.Should().Be(_mockCache.ComponentType);

            model.ProfileId.Should().Be(_mockCache.ProfileId);
            model.AssessmentId.Should().Be(_mockCache.AssessmentId);
            model.IsGradeChanged.Should().BeFalse();

            // Uln
            model.SummaryUln.Title.Should().Be(LearnerDetailsContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(LearnerDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockCache.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Title.Should().Be(LearnerDetailsContent.Title_Provider_Name_Text);
            model.SummaryProviderName.Value.Should().Be(_mockCache.ProviderName);

            // ProviderUkprn
            model.SummaryProviderUkprn.Title.Should().Be(LearnerDetailsContent.Title_Provider_Ukprn_Text);
            model.SummaryProviderUkprn.Value.Should().Be(_mockCache.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TlevelTitle);

            // ExamPeriod
            model.SummaryExamPeriod.Title.Should().Be(LearnerDetailsContent.Title_ExamPeriod_Text);
            model.SummaryExamPeriod.Value.Should().Be(_mockCache.ExamPeriod);

            // Old Grade
            model.SummaryOldGrade.Title.Should().Be(LearnerDetailsContent.Title_Old_Grade);
            model.SummaryOldGrade.Value.Should().Be(_mockCache.OldGrade);

            // New Grade
            model.SummaryNewGrade.Title.Should().Be(LearnerDetailsContent.Title_New_Grade);
            model.SummaryNewGrade.Value.Should().Be(_mockCache.NewGrade);
            model.SummaryNewGrade.ActionText.Should().Be(LearnerDetailsContent.Change_Link);
            model.SummaryNewGrade.HiddenActionText.Should().Be(LearnerDetailsContent.Change_Link_Hidden_Text);
            model.SummaryNewGrade.RouteName.Should().Be(RouteConstants.PrsRommGradeChange);
            model.SummaryNewGrade.RouteAttributes.Should().BeEquivalentTo(RouteParameters);

            // Backlink
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRommOutcomeKnownCoreGrade);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeProfileId);
            routeProfileId.Should().Be(_mockCache.ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string routeAssessmentId);
            routeAssessmentId.Should().Be(_mockCache.AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ComponentType, out string routeComponentType);
            routeComponentType.Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.RommOutcomeKnownTypeId, out string routeRommOutcomeKnownTypeId);
            routeRommOutcomeKnownTypeId.Should().Be(((int)RommOutcomeKnownType.GradeNotChanged).ToString());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
        }
        private Dictionary<string, string> RouteParameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { Constants.ProfileId, _mockCache.ProfileId.ToString() },
                    { Constants.AssessmentId, _mockCache.AssessmentId.ToString() },
                    { Constants.ComponentType, ((int)ComponentType).ToString() },
                    { Constants.IsRommOutcomeJourney, "false" },
                    { Constants.IsChangeMode, "true" }
                };
            }
        }
    }
}
