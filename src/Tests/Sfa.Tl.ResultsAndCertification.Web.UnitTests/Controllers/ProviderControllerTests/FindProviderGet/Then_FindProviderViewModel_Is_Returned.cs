using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderGet
{
    public class Then_FindProviderViewModel_Is_Returned : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            ProviderLoader.IsAnyProviderSetupCompletedAsync(Ukprn).Returns(true);
        }

        [Fact]
        public void Then_IsAnyProviderSetupCompletedAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).IsAnyProviderSetupCompletedAsync(Ukprn);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));
            
            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Should().NotBeNull();
            model.Search.Should().BeNull();
            model.SelectedProviderId.Should().Be(0);
            model.ShowViewProvidersLink.Should().BeTrue();
        }

        [Fact]
        public void Then_Expected_Breadcrumb_Returned()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Should().NotBeNull();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Dashboard);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Provider_Find_Provider);
        }
    }
}
