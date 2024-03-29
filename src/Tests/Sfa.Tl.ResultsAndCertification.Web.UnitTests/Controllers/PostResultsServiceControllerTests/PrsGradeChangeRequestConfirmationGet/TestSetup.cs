﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestConfirmationGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsGradeChangeRequestConfirmationViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsGradeChangeRequestConfirmationAsync();
        }
    }
}
