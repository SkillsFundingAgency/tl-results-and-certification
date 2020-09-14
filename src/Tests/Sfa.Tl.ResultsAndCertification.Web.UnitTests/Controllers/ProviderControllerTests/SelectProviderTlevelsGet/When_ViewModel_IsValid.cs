using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsGet
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private ProviderTlevelsViewModel mockresult;

        public override void Given()
        {
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

            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(mockresult);
        }

        [Fact]
        public void Then_GetSelectProviderTlevelsAsync_Is_Called()
        {
            ProviderLoader.Received().GetSelectProviderTlevelsAsync(Ukprn, ProviderId);
        }

        [Fact]
        public void Then_GetSelectProviderTlevelsAsync_ViewModel_Return_Two_Rows()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ProviderTlevelsViewModel;

            model.Should().NotBeNull();
            model.ProviderId.Should().Be(mockresult.ProviderId);
            model.DisplayName.Should().Be(mockresult.DisplayName);
            model.Ukprn.Should().Be(mockresult.Ukprn);
            model.IsAddTlevel.Should().BeFalse();
            model.Tlevels.Should().NotBeNull();
            model.Tlevels.Count().Should().Be(2);
        }

        [Fact]
        public void Then_GetSelectProviderTlevelsAsync_Index_Returns_Expected_ViewModel()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ProviderTlevelsViewModel;

            model.Tlevels.Should().NotBeNull();

            var expectedFirstItemModel = mockresult.Tlevels.FirstOrDefault();
            var actualFirstItemModel = model.Tlevels.FirstOrDefault();

            expectedFirstItemModel.TqAwardingOrganisationId.Should().Be(actualFirstItemModel.TqAwardingOrganisationId);
            expectedFirstItemModel.TlProviderId.Should().Be(actualFirstItemModel.TlProviderId);
            expectedFirstItemModel.TlevelTitle.Should().Be(actualFirstItemModel.TlevelTitle);
        }
    }
}
