using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadResultsDataFormatAndRulesGuide
{
    public class When_TechSpec_Notfound : TestSetup
    {
        private MemoryStream _memoryStream;

        public override void Given()
        {
            _memoryStream = null;
            DocumentLoader.GetTechSpecFileAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_memoryStream);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
