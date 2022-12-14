using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_BackLink_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration } },
                        new BackLinkModel { RouteName = RouteConstants.IpSpecialConsiderationReasons }
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed } },
                        new BackLinkModel { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, "1" } } }
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.NotCompleted } },
                        new BackLinkModel { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, "1" } } }
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.WillNotComplete } },
                        new BackLinkModel { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, "1" } } }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementViewModel cacheModel, BackLinkModel expectedRoute)
        {
            var viewModel = new IpCheckAndSubmitViewModel { ProfileId = 1 };
            viewModel.SetBackLink(cacheModel);

            viewModel.BackLink.Should().NotBeNull();
            viewModel.BackLink.Should().BeEquivalentTo(expectedRoute);
        }
    }
}
