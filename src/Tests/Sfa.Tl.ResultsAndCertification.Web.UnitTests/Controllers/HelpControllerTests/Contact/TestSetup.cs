﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.HelpControllerTests.Contact
{
    public abstract class TestSetup : BaseTest<HelpController>
    {
        public ResultsAndCertificationConfiguration Configuration { get; set; }
        public IHelpLoader HelpLoader;
        protected IActionResult Result;
        protected HelpController Controller;

        public override void Setup()
        {
            Controller = new HelpController(Configuration, HelpLoader);
        }

        public override Task When()
        {
            Result = Controller.Contact();
            return Task.CompletedTask;
        }
    }
}
