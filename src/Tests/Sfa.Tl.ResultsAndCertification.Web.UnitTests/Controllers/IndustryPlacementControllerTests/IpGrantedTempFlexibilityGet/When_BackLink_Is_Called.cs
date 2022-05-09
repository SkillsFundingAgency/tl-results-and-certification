using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpGrantedTempFlexibilityGet
{
    public class When_BackLink_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = false } }, RouteConstants.IpBlendedPlacementUsed },
                    new object[] { new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = new IpTempFlexibilityUsedViewModel { IsTempFlexibilityUsed = true } }, RouteConstants.IpTempFlexibilityUsed }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IpTempFlexibilityViewModel ipTempFlexModel, string expectedRouteName)
        {
            var viewModel = new IpGrantedTempFlexibilityViewModel();
            viewModel.SetBackLink(ipTempFlexModel);

            viewModel.BackLink.Should().NotBeNull();
            viewModel.BackLink.RouteName.Should().Be(expectedRouteName);
            viewModel.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
