using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Return_Empty_ViewModel : When_Index_Action_Called
    {
        public override void Given()
        {
            var mockresult = new List<YourTlevelsViewModel>();
            TlevelLoader.GetAllTlevelsByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetAllTlevelsByUkprnAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_GetTlevelsByUkprnAsync_ViewModel_Return_Zero_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;
            Assert.True(model.Count == 0);
        }
    }
}
