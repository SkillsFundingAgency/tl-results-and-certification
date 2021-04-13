using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private EnterUlnViewModel _ulnViewModel;

        public override void Given()
        {
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
            model.IsNavigatedFromSearchLearnerRecordNotAdded.Should().BeFalse();
            model.BackLink.RouteName.Should().Be(RouteConstants.ManageLearnerRecordsDashboard);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
