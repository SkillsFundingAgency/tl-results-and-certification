using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.YourProviders
{
    public class Then_YourProvidersViewModel_Is_Returned : When_YourProvidersAsync_Get_Action_Is_Called
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
            var model = viewResult.Model as YourProvidersViewModel; //List<ProviderDetailsViewModel>;

            model.Should().NotBeNull();
            model.Providers.Should().NotBeNull();

            var expectedFirstItemModel = mockresult.Providers.FirstOrDefault();
            var actualFirstItemModel = model.Providers.FirstOrDefault();

            expectedFirstItemModel.ProviderId.Should().Be(actualFirstItemModel.ProviderId);
            expectedFirstItemModel.DisplayName.Should().Be(actualFirstItemModel.DisplayName);
            expectedFirstItemModel.Ukprn.Should().Be(actualFirstItemModel.Ukprn);
        }
    }
}
