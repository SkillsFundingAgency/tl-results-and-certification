using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadResultsTemplate
{
    public class When_TechSpec_Notfound : TestSetup
    {
        public override void Given()
        {
            DocumentLoader.GetTechSpecFileAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(null as MemoryStream);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}