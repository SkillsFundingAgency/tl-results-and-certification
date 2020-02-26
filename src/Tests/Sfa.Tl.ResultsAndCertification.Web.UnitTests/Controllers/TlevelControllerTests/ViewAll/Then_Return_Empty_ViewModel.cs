using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class Then_Return_Empty_ViewModel : When_ViewAll_Action_Called
    {
        public override void Given()
        {
            var mockresult = new List<YourTlevelsViewModel>();
            TlevelLoader.GetAllTlevelsByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetAllTlevelsByUkprnAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_ViewModel_Return_Zero_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as IList<YourTlevelsViewModel>;
            model.Count().Should().Be(0);
        }
    }
}
