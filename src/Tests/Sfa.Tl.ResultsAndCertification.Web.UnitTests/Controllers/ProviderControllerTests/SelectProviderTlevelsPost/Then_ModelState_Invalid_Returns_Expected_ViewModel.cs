using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsPost
{
    public class Then_ModelState_Invalid_Returns_Expected_ViewModel : When_SelectProviderTlevelsAsync_Post_Action_Is_Called
    {
        private ProviderTlevelsViewModel mockresult;

        public override void Given()
        {
            ProviderId = 1;
            mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" }
                }
            };

            InputViewModel = mockresult;
            Controller.ModelState.AddModelError("HasTlevelSelected", Content.Provider.SelectProviderTlevels.Select_Tlevel_Validation_Message);
            ProviderLoader.GetSelectProviderTlevelsAsync(Ukprn, ProviderId).Returns(mockresult);
        }

        [Fact]
        public void Then_GetSelectProviderTlevelsAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).GetSelectProviderTlevelsAsync(Ukprn, ProviderId);
        }

        [Fact]
        public void Then_On_ModelState_Not_Valid_Expected_ViewModel_Is_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var actualViewModel = viewResult.Model as ProviderTlevelsViewModel;

            actualViewModel.Should().NotBeNull();
            actualViewModel.ProviderId.Should().Be(mockresult.ProviderId);
            actualViewModel.DisplayName.Should().Be(mockresult.DisplayName);
            actualViewModel.IsAddTlevel.Should().BeFalse();
            actualViewModel.Ukprn.Should().Be(mockresult.Ukprn);
            actualViewModel.Tlevels.Should().NotBeNull();
            actualViewModel.Tlevels.Count().Should().Be(2);

            var expectedFirstItemModel = mockresult.Tlevels.FirstOrDefault();
            var actualFirstItemModel = actualViewModel.Tlevels.FirstOrDefault();

            actualFirstItemModel.TqAwardingOrganisationId.Should().Be(expectedFirstItemModel.TqAwardingOrganisationId);
            actualFirstItemModel.TlProviderId.Should().Be(expectedFirstItemModel.TlProviderId);
            actualFirstItemModel.TlevelTitle.Should().Be(expectedFirstItemModel.TlevelTitle);
        }
    }
}
