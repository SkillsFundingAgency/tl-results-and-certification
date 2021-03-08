using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddIndustryPlacementQuestionGet
{
    public class When_NoCache_Found_For_LearnerRecord : TestSetup
    {
        private AddLearnerRecordViewModel cacheResult;
        private EnterUlnViewModel _ulnViewModel;

        public override void Given()
        {
            _ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };

            cacheResult = new AddLearnerRecordViewModel
            {
                Uln = _ulnViewModel
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
