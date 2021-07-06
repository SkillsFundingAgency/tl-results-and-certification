using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsPathwayGradeCheckAndSubmitGet
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private PrsPathwayGradeCheckAndSubmitViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new PrsPathwayGradeCheckAndSubmitViewModel
            {
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "Tlevel in Education",
                PathwayTitle = "Educateion (1234455)",
                ProviderName = "Barsley College",
                ProviderUkprn = 87654321,
                NewGrade = "A",
                OldGrade = "B",
                
                ProfileId = 1,
                AssessmentId = 2,
                ResultId= 3,
            };
            CacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsPathwayGradeCheckAndSubmitViewModel;

            model.Should().NotBeNull();
            model.Firstname.Should().Be(_mockCache.Firstname);
            model.Lastname.Should().Be(_mockCache.Lastname);
            model.LearnerName.Should().Be($"{_mockCache.Firstname} {_mockCache.Lastname}");
            model.ProviderName.Should().Be(_mockCache.ProviderName);
            model.ProviderUkprn.Should().Be(_mockCache.ProviderUkprn);
            model.ProviderDisplayName.Should().Be($"{_mockCache.ProviderName}<br/>({_mockCache.ProviderUkprn})");
            model.TlevelTitle.Should().Be(_mockCache.TlevelTitle);
            model.NewGrade.Should().Be(_mockCache.NewGrade);
            model.OldGrade.Should().Be(_mockCache.OldGrade);

            model.ProfileId.Should().Be(_mockCache.ProfileId); 
            model.AssessmentId.Should().Be(_mockCache.AssessmentId);
            model.ResultId.Should().Be(_mockCache.ResultId);

            // Summary


            // Backlink
            //model.BackLink.Should().NotBeNull();
            //model.BackLink.RouteName.Should().Be(RouteConstants.PrsSearchLearner);
            //model.BackLink.RouteAttributes.Count.Should().Be(1);
            //model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            //routeValue.Should().Be(true.ToString());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
        }
    }
}
