using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private string _searchUln = "9895641231";

        public override void Given() 
        {
            CacheService.GetAndRemoveAsync<string>(Arg.Any<string>()).Returns(_searchUln);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as EnterUlnViewModel;
            model.BackLink.Should().NotBeNull();
            model.EnterUln.Should().Be(_searchUln);
            model.BackLink.RouteName.Should().Be(RouteConstants.ManageLearnerRecordsDashboard);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
