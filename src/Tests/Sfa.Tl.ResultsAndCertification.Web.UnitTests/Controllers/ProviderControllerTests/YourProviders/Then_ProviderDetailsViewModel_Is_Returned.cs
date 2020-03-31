using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.YourProviders
{
    public class Then_ProviderDetailsViewModel_Is_Returned : When_YourProvidersAsync_Get_Action_Is_Called
    {
        private List<ProviderDetailsViewModel> mockresult;

        public override void Given()
        {
            Ukprn = 10011881;
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            mockresult = new List<ProviderDetailsViewModel>
            {
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
            };

            ProviderLoader.GetTqAoProviderDetailsAsync(Ukprn).Returns(mockresult);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_Is_Called()
        {
            ProviderLoader.Received().GetTqAoProviderDetailsAsync(Ukprn);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as List<ProviderDetailsViewModel>;

            model.Should().NotBeNull();
            model.Count().Should().Be(2);
        }

        [Fact]
        public void Then_YourProviders_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as List<ProviderDetailsViewModel>;

            model.Should().NotBeNull();

            var expectedFirstItemModel = mockresult.FirstOrDefault();
            var actualFirstItemModel = model.FirstOrDefault();

            expectedFirstItemModel.ProviderId.Should().Be(actualFirstItemModel.ProviderId);
            expectedFirstItemModel.DisplayName.Should().Be(actualFirstItemModel.DisplayName);
            expectedFirstItemModel.Ukprn.Should().Be(actualFirstItemModel.Ukprn);
        }
    }
}
