﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberGet
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.RequestSoaUniqueLearnerNumberAsync();
        }
    }
}
