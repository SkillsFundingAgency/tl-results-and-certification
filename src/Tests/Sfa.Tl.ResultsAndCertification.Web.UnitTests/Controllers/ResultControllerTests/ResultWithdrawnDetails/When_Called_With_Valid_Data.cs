using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultWithdrawnDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ResultWithdrawnViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultWithdrawnViewModel
            {
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.Now.AddYears(-30),
                TlevelTitle = "Tlevel title",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567891
            };

            ResultLoader.GetResultWithdrawnViewModelAsync(AoUkprn, ProfileId).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultWithdrawnViewModelAsync(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultWithdrawnViewModel));

            var model = viewResult.Model as ResultWithdrawnViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(_mockResult.Uln);
            model.Firstname.Should().Be(_mockResult.Firstname);
            model.Lastname.Should().Be(_mockResult.Lastname);
            model.LearnerName.Should().Be($"{_mockResult.Firstname} {_mockResult.Lastname}");
            model.DateofBirth.Should().Be(_mockResult.DateofBirth);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            model.ProviderDisplayName.Should().Be($"{_mockResult.ProviderName}<br/>({_mockResult.ProviderUkprn})");

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchResults);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }
    }
}
