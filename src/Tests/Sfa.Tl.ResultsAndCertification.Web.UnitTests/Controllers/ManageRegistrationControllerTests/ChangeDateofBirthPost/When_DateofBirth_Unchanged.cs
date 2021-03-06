﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeDateofBirthPost
{
    public class When_DateofBirth_Unchanged : TestSetup
    {
        public override void Given()
        {
            MockResult.IsModified = false;

            RegistrationLoader.ProcessDateofBirthChangeAsync(AoUkprn, ViewModel)
                .Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationDetails);
        }
    }
}
