using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedGet
{
    public class When_BackLink_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } }, RouteConstants.IpModelUsed },
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true } }, RouteConstants.IpMultiEmployerOther },
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = false } }, RouteConstants.IpMultiEmployerSelect },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IpModelViewModel ipModel, string expectedRouteName)
        {
            var viewModel = new IpTempFlexibilityUsedViewModel();
            viewModel.SetBackLink(ipModel);

            viewModel.BackLink.Should().NotBeNull();
            viewModel.BackLink.RouteName.Should().Be(expectedRouteName);
            viewModel.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
