﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.SearchRegistrationLoaderTests.GetSearchRegistrationDetailsListTests
{
    public class When_Is_Widthdrawn_Result_Returns_Expected : TestSetup
    {
        public override void Given()
            => Given(SearchRegistrationType.Result, isWithdrawn: true, hasResults: true);

        [Fact]
        public void Then_Expected_Methods_Called()
            => AssertExpectedMethodsCalled();

        [Fact]
        public void Then_Returns_Expected()
        {
            AssertResultExceptRouteName();
            Result.RegistrationDetails[0].Route.RouteName.Should().Be(RouteConstants.ResultWithdrawnDetails);
        }
    }
}