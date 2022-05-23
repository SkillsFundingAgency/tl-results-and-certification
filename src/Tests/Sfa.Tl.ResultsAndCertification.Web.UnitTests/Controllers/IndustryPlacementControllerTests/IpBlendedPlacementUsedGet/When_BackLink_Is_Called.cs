using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedGet
{
    public class When_BackLink_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new IpModelViewModel(), true, RouteConstants.IpTempFlexibilityUsed },
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } }, false, RouteConstants.IpModelUsed },
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true } }, false, RouteConstants.IpMultiEmployerOther },
                    new object[] { new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = false } }, false, RouteConstants.IpMultiEmployerSelect },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IpModelViewModel ipModel, bool isTempFlexApplicable, string expectedRouteName)
        {
            var viewModel = new IpBlendedPlacementUsedViewModel();
            viewModel.SetBackLink(ipModel, isTempFlexApplicable);

            viewModel.BackLink.Should().NotBeNull();
            viewModel.BackLink.RouteName.Should().Be(expectedRouteName);
            viewModel.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
