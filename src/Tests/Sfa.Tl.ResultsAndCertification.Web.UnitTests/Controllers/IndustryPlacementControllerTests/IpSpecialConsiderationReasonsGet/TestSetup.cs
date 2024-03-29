﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationReasonsGet
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public bool IsChangeMode { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.IpSpecialConsiderationReasonsAsync();
        }
    }
}
