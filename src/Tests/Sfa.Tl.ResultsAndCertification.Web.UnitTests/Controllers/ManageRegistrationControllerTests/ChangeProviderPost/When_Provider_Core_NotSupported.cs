﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeProviderPost
{
    public class When_Provider_Core_NotSupported : TestSetup
    {
        public override void Given()
        {
            MockResult.IsModified = true;
            MockResult.IsCoreNotSupported = true;
            RegistrationLoader.ProcessProviderChangesAsync(AoUkprn, ViewModel).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_CannotChangeRegistrationProvider()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationCoreQuestion);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
