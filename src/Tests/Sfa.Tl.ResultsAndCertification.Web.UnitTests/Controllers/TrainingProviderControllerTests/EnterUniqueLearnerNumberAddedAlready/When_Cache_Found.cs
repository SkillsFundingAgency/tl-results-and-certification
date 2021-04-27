using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerNumberAddedAlready
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private AddLearnerRecordViewModel mockCache = null;

        public override void Given()
        {
            ProfileId = 1;            
            mockCache = new AddLearnerRecordViewModel { LearnerRecord = new FindLearnerRecord { Name = "Test user" },  Uln = new EnterUlnViewModel { EnterUln = uln.ToString() } };
            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey)
                .Returns(mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as LearnerRecordAddedAlreadyViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ProfileId);
            model.Uln.Should().Be(uln.ToString());
            model.LearnerName.Should().Be(mockCache.LearnerRecord.Name);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumber);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
        }
    }
}
