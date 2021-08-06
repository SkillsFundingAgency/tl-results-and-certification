using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SelectAssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsSelectAssessmentSeries;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearnerRecord;
        private PrsSelectAssessmentSeriesViewModel _prsSelectAssessmentSeriesViewModel;

        public override void Given()
        {
            ViewModel = new PrsSelectAssessmentSeriesViewModel { Uln = 1234567890, SelectedAssessmentId = null };
            
            _findPrsLearnerRecord = new FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = ViewModel.Uln,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-18),
                TlevelTitle = "Tlevel in Construction",
                ProviderName = "Barsley college",
                ProviderUkprn = 12345678,
                Status = RegistrationPathwayStatus.Active,
                PathwayAssessments = new List<PrsAssessment>
                {
                    new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                    new PrsAssessment { AssessmentId = 22, SeriesName = "Autumn 2021", HasResult = false }
                }
            };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln).Returns(_findPrsLearnerRecord);

            _prsSelectAssessmentSeriesViewModel = new PrsSelectAssessmentSeriesViewModel
            {
                Uln = _findPrsLearnerRecord.Uln,
                Firstname = _findPrsLearnerRecord.Firstname,
                Lastname = _findPrsLearnerRecord.Lastname,
                DateofBirth = _findPrsLearnerRecord.DateofBirth,
                ProviderName = _findPrsLearnerRecord.ProviderName,
                ProviderUkprn = _findPrsLearnerRecord.ProviderUkprn,
                TlevelTitle = _findPrsLearnerRecord.TlevelTitle,
                AssessmentSerieses = _findPrsLearnerRecord.PathwayAssessments.ToList()
            };
            Loader.TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(_findPrsLearnerRecord).Returns(_prsSelectAssessmentSeriesViewModel);

            Controller.ModelState.AddModelError("SelectedAssessmentId", SelectAssessmentContent.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsSelectAssessmentSeriesViewModel));

            var model = viewResult.Model as PrsSelectAssessmentSeriesViewModel;
            model.Should().NotBeNull();
            model.Uln.Should().Be(_prsSelectAssessmentSeriesViewModel.Uln);
            model.Firstname.Should().Be(_prsSelectAssessmentSeriesViewModel.Firstname);
            model.Lastname.Should().Be(_prsSelectAssessmentSeriesViewModel.Lastname);
            model.DateofBirth.Should().Be(_prsSelectAssessmentSeriesViewModel.DateofBirth);
            model.ProviderName.Should().Be(_prsSelectAssessmentSeriesViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(_prsSelectAssessmentSeriesViewModel.ProviderUkprn);
            model.TlevelTitle.Should().Be(_prsSelectAssessmentSeriesViewModel.TlevelTitle);
            model.SelectedAssessmentId.Should().BeNull();
            model.AssessmentSerieses.Count().Should().Be(_prsSelectAssessmentSeriesViewModel.AssessmentSerieses.Count());

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsSelectAssessmentSeriesViewModel.SelectedAssessmentId)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsSelectAssessmentSeriesViewModel.SelectedAssessmentId)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectAssessmentContent.Validation_Message);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln);
            Loader.Received(1).TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(_findPrsLearnerRecord);
        }
    }
}
