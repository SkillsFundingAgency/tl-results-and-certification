using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PrsSelectSeriesContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsSelectAssessmentSeries;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesGet
{
    public class When_Learner_IsValid : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearner;
        private PrsSelectAssessmentSeriesViewModel _prsSelectAssessmentSeriesViewModel;

        public override void Given()
        {
            ProfileId = 1;
            _findPrsLearner = new FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Provider",
                ProviderUkprn = 7894561231,
                TlevelTitle = "Title",
                Status = Common.Enum.RegistrationPathwayStatus.Active,
                PathwayAssessments = new List<PrsAssessment>
                {
                    new PrsAssessment { HasResult = true },
                    new PrsAssessment { HasResult = false }
                }
            };

            _prsSelectAssessmentSeriesViewModel = new PrsSelectAssessmentSeriesViewModel
            {
                Uln = _findPrsLearner.Uln,
                Firstname = _findPrsLearner.Firstname,
                Lastname = _findPrsLearner.Lastname,
                DateofBirth = _findPrsLearner.DateofBirth,
                ProviderName = _findPrsLearner.ProviderName,
                ProviderUkprn = _findPrsLearner.ProviderUkprn,
                TlevelTitle = _findPrsLearner.TlevelTitle,
                SelectedAssessmentId = null,
                AssessmentSerieses = _findPrsLearner.PathwayAssessments.ToList()
            };

            Loader.FindPrsLearnerRecordAsync(AoUkprn, null, ProfileId).Returns(_findPrsLearner);
            Loader.TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(_findPrsLearner).Returns(_prsSelectAssessmentSeriesViewModel);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, null, ProfileId);
            Loader.Received(1).TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(_findPrsLearner);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
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

            foreach (var actual in model.AssessmentSerieses.Select((value, i) => (value, i)))
            {
                actual.value.AssessmentId.Should().Be(_prsSelectAssessmentSeriesViewModel.AssessmentSerieses[actual.i].AssessmentId);
                actual.value.SeriesName.Should().Be(_prsSelectAssessmentSeriesViewModel.AssessmentSerieses[actual.i].SeriesName);
                actual.value.HasResult.Should().Be(_prsSelectAssessmentSeriesViewModel.AssessmentSerieses[actual.i].HasResult);
            }

            // Uln
            model.SummaryUln.Title.Should().Be(PrsSelectSeriesContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_prsSelectAssessmentSeriesViewModel.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(PrsSelectSeriesContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_prsSelectAssessmentSeriesViewModel.Firstname} {_prsSelectAssessmentSeriesViewModel.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(PrsSelectSeriesContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_prsSelectAssessmentSeriesViewModel.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(PrsSelectSeriesContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_prsSelectAssessmentSeriesViewModel.ProviderName}<br/>({_prsSelectAssessmentSeriesViewModel.ProviderUkprn})");

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(PrsSelectSeriesContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_prsSelectAssessmentSeriesViewModel.TlevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }
    }
}
