using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelAppealUpdateGet
{
    public class When_Cache_Found : TestSetup
    {
        private PrsPathwayGradeCheckAndSubmitViewModel _cacheResult;

        public override void Given()
        {

            _cacheResult = new PrsPathwayGradeCheckAndSubmitViewModel
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
                IsGradeChanged = false,

                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
            };

            CacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsCancelAppealUpdateViewModel));

            var model = viewResult.Model as PrsCancelAppealUpdateViewModel;
            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_cacheResult.ProfileId);
            model.AssessmentId.Should().Be(_cacheResult.AssessmentId);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsPathwayGradeCheckAndSubmit);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
