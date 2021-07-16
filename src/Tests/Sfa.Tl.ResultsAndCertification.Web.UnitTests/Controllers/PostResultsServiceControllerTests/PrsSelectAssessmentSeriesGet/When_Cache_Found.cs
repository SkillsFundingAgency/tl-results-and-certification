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
    public class When_Cache_Found : TestSetup
    {
        private readonly long _uln = 1234567890;
        private PrsSelectAssessmentSeriesViewModel _mockCache;

        public override void Given()
        {
            _mockCache = new PrsSelectAssessmentSeriesViewModel
            {
                Uln = _uln,
                Firstname = "Test",
                Lastname = "User",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Provider",
                ProviderUkprn = 985647841,
                TlevelTitle = "Title",
                AssessmentSerieses = new List<PrsAssessment>
                {
                    new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                    new PrsAssessment { AssessmentId = 22, SeriesName = "Autumn 2021", HasResult = false },
                }
            };
            CacheService.GetAndRemoveAsync<PrsSelectAssessmentSeriesViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact(Skip = "Ravi:todo")]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsSelectAssessmentSeriesViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(_mockCache.Uln);
            model.Firstname.Should().Be(_mockCache.Firstname);
            model.Lastname.Should().Be(_mockCache.Lastname);
            model.DateofBirth.Should().Be(_mockCache.DateofBirth);
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.ProviderUkprn.Should().Be(_mockCache.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockCache.TlevelTitle);
            model.SelectedAssessmentId.Should().BeNull();
            model.AssessmentSerieses.Count().Should().Be(_mockCache.AssessmentSerieses.Count());

            foreach (var actual in model.AssessmentSerieses.Select((value, i) => (value, i)))
            {
                actual.value.AssessmentId.Should().Be(_mockCache.AssessmentSerieses[actual.i].AssessmentId);
                actual.value.SeriesName.Should().Be(_mockCache.AssessmentSerieses[actual.i].SeriesName);
                actual.value.HasResult.Should().Be(_mockCache.AssessmentSerieses[actual.i].HasResult);
            }

            // Uln
            model.SummaryUln.Title.Should().Be(PrsSelectSeriesContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockCache.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(PrsSelectSeriesContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_mockCache.Firstname} {_mockCache.Lastname}");

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(PrsSelectSeriesContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockCache.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProvider.Title.Should().Be(PrsSelectSeriesContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be($"{_mockCache.ProviderName}<br/>({_mockCache.ProviderUkprn})");

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(PrsSelectSeriesContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockCache.TlevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }

        [Fact(Skip = "Ravi:todo")]
        public void Then_Expected_Methods_AreCalled()
        {
        }
    }
}
