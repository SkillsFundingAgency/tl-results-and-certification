using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class Then_On_Success_Call_GetTqAoProviderDetailsAsync : When_RemoveProviderTlevelAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            Ukprn = 1000001;
            TqProviderId = 1;
            TlProviderId = 5;

            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            TempData = new TempDataDictionary(HttpContextAccessor.HttpContext, Substitute.For<ITempDataProvider>());
            Controller.TempData = TempData;

            ViewModel = new ProviderTlevelDetailsViewModel { Id = TqProviderId, TlProviderId = TlProviderId, CanRemoveTlevel = true, ShowBackToProvidersLink = true };
            ProviderLoader.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId).Returns(true);

            var mockresult = new List<ProviderDetailsViewModel>
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
        public void Then_GetTqAoProviderDetailsAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).GetTqAoProviderDetailsAsync(Ukprn);
        }
    }
}
