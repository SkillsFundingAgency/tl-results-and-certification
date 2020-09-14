using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.YourProviders
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private YourProvidersViewModel mockresult;

        public override void Given()
        {
            Ukprn = 10011881;
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
            mockresult = new YourProvidersViewModel
            {
                Providers = new List<ProviderDetailsViewModel> {
                    new ProviderDetailsViewModel
                    {
                        ProviderId = 1,
                        DisplayName = "Test",
                        Ukprn = 10000111
                    },
                    new ProviderDetailsViewModel
                    {
                        ProviderId = 2,
                        DisplayName = "Display",
                        Ukprn = 10000112
                    }
                }
            };

            ProviderLoader.GetYourProvidersAsync(Ukprn).Returns(mockresult);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_Is_Called()
        {
            ProviderLoader.Received().GetYourProvidersAsync(Ukprn);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as YourProvidersViewModel; //List<ProviderDetailsViewModel>;

            model.Should().NotBeNull();
            model.Providers.Should().NotBeNull();
            model.Providers.Count().Should().Be(2);
        }

        [Fact]
        public void Then_YourProviders_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as YourProvidersViewModel;

            model.Should().NotBeNull();
            model.Providers.Should().NotBeNull();

            var expectedFirstItemModel = mockresult.Providers.FirstOrDefault();
            var actualFirstItemModel = model.Providers.FirstOrDefault();

            expectedFirstItemModel.ProviderId.Should().Be(actualFirstItemModel.ProviderId);
            expectedFirstItemModel.DisplayName.Should().Be(actualFirstItemModel.DisplayName);
            expectedFirstItemModel.Ukprn.Should().Be(actualFirstItemModel.Ukprn);

            // Breadcrumb

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Provider_Your_Providers);
        }
    }
}
