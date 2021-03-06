﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNamePost
{
    public class When_NameChange_Sucess : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = true;
            MockResult.IsModified = true;

            RegistrationLoader.ProcessProfileNameChangeAsync(AoUkprn, ViewModel)
                .Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_ChangeRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationConfirmation);
        }
    }
}
