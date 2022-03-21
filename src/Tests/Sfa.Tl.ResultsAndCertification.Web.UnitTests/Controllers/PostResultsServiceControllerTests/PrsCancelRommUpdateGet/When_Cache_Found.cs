using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelRommUpdateGet
{
    public class When_Cache_Found : TestSetup
    {
        private PrsRommCheckAndSubmitViewModel _cacheResult;

        public override void Given()
        {
            var previousGrade = "A";
            var newGrade = "A";
            ComponentType = ComponentType.Specialism;

            _cacheResult = new PrsRommCheckAndSubmitViewModel
            {
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "Tlevel in Education",
                ProviderName = "Barsley College",
                ProviderUkprn = 87654321,
                SpecialismName = "Education",
                SpecialismLarId = "1234567",
                ExamPeriod = "Summer 2021",
                NewGrade = newGrade,
                OldGrade = previousGrade,
                IsGradeChanged = false,
                ComponentType = ComponentType,

                ProfileId = 1,
                AssessmentId = 2
            };

            CacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsCancelRommUpdateViewModel));

            var model = viewResult.Model as PrsCancelRommUpdateViewModel;
            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_cacheResult.ProfileId);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsRommCheckAndSubmit);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
