﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultsDownloadDataLink
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            Id = "xyz";
            ComponentType = Common.Enum.ComponentType.Core;
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.Error);
        }
    }
}
