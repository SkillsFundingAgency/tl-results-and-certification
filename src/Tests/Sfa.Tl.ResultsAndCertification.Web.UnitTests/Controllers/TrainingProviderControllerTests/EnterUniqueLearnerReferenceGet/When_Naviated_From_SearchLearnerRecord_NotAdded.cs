using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceGet
{
    public class When_Naviated_From_SearchLearnerRecord_NotAdded : TestSetup
    {
        private EnterUlnViewModel _ulnViewModel;

        public override void Given()
        {
            IsNavigatedFromSearchLearnerRecordNotAdded = true;
            _ulnViewModel = new EnterUlnViewModel { EnterUln = "9895641231" };
            var cacheModel = new AddLearnerRecordViewModel { Uln = _ulnViewModel };
            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as EnterUlnViewModel;
            model.BackLink.Should().NotBeNull();
            model.EnterUln.Should().Be(_ulnViewModel.EnterUln);
            model.IsNavigatedFromSearchLearnerRecordNotAdded.Should().BeTrue();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchLearnerRecordNotAdded);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
