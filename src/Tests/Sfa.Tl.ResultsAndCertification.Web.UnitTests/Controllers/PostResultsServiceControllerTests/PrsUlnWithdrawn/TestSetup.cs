﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsUlnWithdrawn
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsUlnWithdrawnAsync();
        }
    }
}