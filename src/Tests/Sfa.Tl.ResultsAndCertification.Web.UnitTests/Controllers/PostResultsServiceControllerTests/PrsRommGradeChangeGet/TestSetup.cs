﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommGradeChangeGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public ComponentType ComponentType { get; set; }
        public bool? IsRommOutcomeJourney { get; set; }
        public bool? IsChangeMode { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsRommGradeChangeAsync(ProfileId, AssessmentId, ComponentType, IsRommOutcomeJourney, IsChangeMode);
        }
    }
}
