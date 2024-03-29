﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommGet
{
    public class When_Called_With_IsBack_True_For_Specialism : TestSetup
    {
        private PrsAddRommViewModel _addRommViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;
            IsBack = true;

            _addRommViewModel = new PrsAddRommViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                SpecialismName = "Childcare",
                SpecialismLarId = "12121212",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                GradeCode = "PCG2",
                PrsStatus = null,
                ComponentType = ComponentType,
                RommEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddRommViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addRommViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommViewModel.Uln);
            model.LearnerName.Should().Be(_addRommViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommViewModel.TlevelTitle);
            model.SpecialismName.Should().Be(_addRommViewModel.SpecialismName);
            model.SpecialismLarId.Should().Be(_addRommViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addRommViewModel.SpecialismName} ({_addRommViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addRommViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommViewModel.ComponentType);
            model.IsRommRequested.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
