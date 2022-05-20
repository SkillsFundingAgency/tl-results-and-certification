using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private SearchLearnerDetailsListViewModel _searchLearnersList;

        public override void Given()
        {
            AcademicYear = 2020;
            TrainingProviderLoader.SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear).Returns(_searchLearnersList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
