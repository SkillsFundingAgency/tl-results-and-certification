﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultsDownloadData
{
    public class When_Cache_NotFound : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            // Controller
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
