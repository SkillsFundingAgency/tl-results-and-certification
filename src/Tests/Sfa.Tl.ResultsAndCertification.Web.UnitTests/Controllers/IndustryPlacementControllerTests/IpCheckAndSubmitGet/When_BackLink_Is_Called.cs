using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
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
                        new IndustryPlacementViewModel { IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true } } },
                        null, // navigation
                        RouteConstants.IpModelUsed
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true  } } },
                        null, // navigation
                        RouteConstants.IpMultiEmployerOther
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = false  } } },
                        null, // navigation
                        RouteConstants.IpMultiEmployerSelect
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = new IpTempFlexibilityUsedViewModel { IsTempFlexibilityUsed = false } } },
                        new  IpTempFlexNavigation { AskTempFlexibility = true, AskBlendedPlacement = false },
                        RouteConstants.IpTempFlexibilityUsed
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel(),
                        new  IpTempFlexNavigation { AskTempFlexibility = true, AskBlendedPlacement = false },
                        RouteConstants.IpGrantedTempFlexibility
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { TempFlexibility = new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel() } },
                        new  IpTempFlexNavigation { AskTempFlexibility = false, AskBlendedPlacement = true },
                        RouteConstants.IpBlendedPlacementUsed
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { TempFlexibility = new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel(), IpEmployerLedUsed = new IpEmployerLedUsedViewModel() } },
                        new  IpTempFlexNavigation { AskTempFlexibility = false, AskBlendedPlacement = true },
                        RouteConstants.IpEmployerLedUsed
                    },

                    new object[]
                    {
                        new IndustryPlacementViewModel { TempFlexibility = new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel(), IpGrantedTempFlexibility = new IpGrantedTempFlexibilityViewModel() } },
                        new  IpTempFlexNavigation { AskTempFlexibility = false, AskBlendedPlacement = true },
                        RouteConstants.IpGrantedTempFlexibility
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementViewModel cacheModel, IpTempFlexNavigation navigation, string expectedRouteName)
        {
            var viewModel = new IpCheckAndSubmitViewModel();
            viewModel.SetBackLink(cacheModel, navigation);

            viewModel.BackLink.Should().NotBeNull();
            viewModel.BackLink.RouteName.Should().Be(expectedRouteName);
            viewModel.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
