﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DashboardControllerTests.Index
{
    public abstract class TestSetup : BaseTest<DashboardController>
    {
        protected IDashboardLoader Loader;
        protected ILogger<DashboardController> Logger;
        protected DashboardController Controller;
        protected IActionResult Result;
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            Loader = Substitute.For<IDashboardLoader>();
            Logger = Substitute.For<ILogger<DashboardController>>();
            Controller = new DashboardController(Loader, Logger);
        }

        public override async Task When()
        {
            Result = await Controller.Index();
        }
    }
}
