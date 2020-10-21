using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderGet
{
    public class When_SearchResults_Empty : When_FindProviderAsync_Post_Action_Is_Called
    {
        private Task<IActionResult> _selectProviderTlevelResult;
        private ProviderTlevelsViewModel _mockresult;
        private string _searchText = "Test College";

        public override void Given()
        {
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            ProviderLoader.IsAnyProviderSetupCompletedAsync(Ukprn).Returns(true);

            _mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = _searchText,
                Ukprn = 10000111,
                IsAddTlevel = true,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" }
                }
            };

            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(_mockresult);
        }
        public async override Task When()
        {
            _selectProviderTlevelResult = Controller.AddProviderTlevelsAsync(1);
            Result = await Controller.FindProviderAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Should().NotBeNull();
            model.Search.Should().BeNull();
        }
    }
}
