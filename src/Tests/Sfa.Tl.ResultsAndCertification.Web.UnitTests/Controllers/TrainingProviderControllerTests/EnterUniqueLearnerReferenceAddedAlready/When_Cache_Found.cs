using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceAddedAlready
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;

        public override void Given()
        {
            CacheService.GetAndRemoveAsync<LearnerRecordAddedAlreadyViewModel>(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberAddedAlready))
                .Returns(new LearnerRecordAddedAlreadyViewModel { Uln = uln.ToString() });
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as LearnerRecordAddedAlreadyViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(uln.ToString());

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumber);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}
