using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevels
{
    public class Then_Return_TwoRecord_ViewModel : When_SelectProviderTlevelsAsync_Get_Action_Is_Called
    {
        private ProviderTlevelsViewModel mockresult;

        public override void Given()
        {
            mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelDetailsViewModel>
                {
                    new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 1, RouteName = "Route1", PathwayName = "Pathway1"},
                    new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 2, RouteName = "Route2", PathwayName = "Pathway2"}
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
            expectedFirstItemModel.ProviderId.Should().Be(actualFirstItemModel.ProviderId);
            expectedFirstItemModel.PathwayId.Should().Be(actualFirstItemModel.PathwayId);
            expectedFirstItemModel.RouteName.Should().Be(actualFirstItemModel.RouteName);
            expectedFirstItemModel.PathwayName.Should().Be(actualFirstItemModel.PathwayName);
            expectedFirstItemModel.TlevelTitle.Should().Be(actualFirstItemModel.TlevelTitle);
        }
    }
}
