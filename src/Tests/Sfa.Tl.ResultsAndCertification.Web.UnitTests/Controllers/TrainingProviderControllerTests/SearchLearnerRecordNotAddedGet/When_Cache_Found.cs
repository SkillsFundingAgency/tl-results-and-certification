using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordNotAddedGet
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private SearchLearnerRecordViewModel cacheModel = null;

        public override void Given()
        {
            cacheModel = new SearchLearnerRecordViewModel { SearchUln = uln.ToString(), IsLearnerRecordAdded = false };
            CacheService.GetAsync<SearchLearnerRecordViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as LearnerRecordNotAddedViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(cacheModel.SearchUln);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchLearnerRecordViewModel>(CacheKey);
        }
    }
}
