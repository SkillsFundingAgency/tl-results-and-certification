using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class Then_PageNotFound_Error_On_Null_Result : When_Details_Action_Called
    {
        private YourTLevelDetailsViewModel mockresult;

        public override void Given()
        {
            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(ukPrn, id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_PageNotFound_Is_Returned_OnNull_Result()
        {
            Assert.Same((Result.Result as RedirectToActionResult).ActionName, nameof(ErrorController.PageNotFound));
        }
    }
}
