using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.ViewProviderTlevels
{
    public class Then_Expected_Results_Are_Returned : When_ViewProviderTlevelsAsync_Is_Called
    {
        private ProviderViewModel mockViewmodel;

        public override void Given()
        {
            mockViewmodel = new ProviderViewModel
            {
                DisplayName = "Kings College",
                Ukprn = 1234,
                ProviderId = 24,
                Tlevels = new List<TlevelViewModel> 
                {
                    new TlevelViewModel { TlevelTitle = "Design: Construction" },
                    new TlevelViewModel { TqProviderId = 99, TlevelTitle = "Childcare" },
                    new TlevelViewModel { TqProviderId = 98, TlevelTitle = "Education" },
                    new TlevelViewModel { TlevelTitle = "Arts" },
                }, 
            };

            ProviderLoader.GetViewProviderTlevelViewModelAsync(Arg.Any<long>(), providerId)
                .Returns(mockViewmodel);
        }

        [Fact]
        public void Then_Loader_GetViewProviderTlevelViewModelAsync_Is_Called()
        {
            ProviderLoader.Received(1).GetViewProviderTlevelViewModelAsync(Arg.Any<long>(), providerId);
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Returned()
        {
            var actualResult = Result.Result;
            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType(typeof(ViewResult));

            var viewResult = actualResult as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ProviderViewModel));

            var resultModel = viewResult.Model as ProviderViewModel;
            resultModel.Should().NotBeNull();
            resultModel.ShowAnotherTlevelButton.Should().BeTrue();
            resultModel.ProviderId.Should().Be(providerId);
            resultModel.AnyTlevelsAvailable.Should().BeTrue();
            resultModel.DisplayName.Should().Be(mockViewmodel.DisplayName);
            resultModel.IsNavigatedFromFindProvider.Should().BeFalse();
            resultModel.Tlevels.Should().NotBeNullOrEmpty();
            resultModel.Tlevels.Count().Should().Be(mockViewmodel.Tlevels.Count);
            resultModel.ProviderTlevels.Should().NotBeNullOrEmpty();
            resultModel.ProviderTlevels.Count().Should().Be(2);
            resultModel.ProviderTlevels.First().TqProviderId.Should().Be(99);
            resultModel.ProviderTlevels.First().TlevelTitle.Should().Be("Childcare");

            // Breadcrumb
            resultModel.Breadcrumb.Should().NotBeNull();
            resultModel.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            resultModel.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            resultModel.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            resultModel.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            resultModel.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.YourProviders);
            resultModel.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Provider_Your_Providers);
            resultModel.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNullOrEmpty();
            resultModel.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Provider_Tlevels);
        }
    }
}
