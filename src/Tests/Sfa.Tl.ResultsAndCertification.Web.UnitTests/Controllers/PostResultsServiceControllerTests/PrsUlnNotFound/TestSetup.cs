﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsUlnNotFound
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsUlnNotFoundAsync();
        }
    }
}
