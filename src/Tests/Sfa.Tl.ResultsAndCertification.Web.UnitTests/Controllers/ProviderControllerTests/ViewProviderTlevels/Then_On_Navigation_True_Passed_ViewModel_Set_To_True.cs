using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.ViewProviderTlevels
{
    public class Then_On_Navigation_True_Passed_ViewModel_Set_To_True : When_ViewProviderTlevelsAsync_Is_Called
    {
        private ProviderViewModel mockViewmodel;

        public override void Given()
        {
            navigation = true;

            mockViewmodel = new ProviderViewModel()
            {
                Tlevels = new List<TlevelViewModel>()
                {
                    new TlevelViewModel { TlevelTitle = "Arts" }
                },
            };

            ProviderLoader.GetViewProviderTlevelViewModelAsync(Arg.Any<long>(), providerId)
                .Returns(mockViewmodel);
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
            resultModel.IsNavigatedFromFindProvider.Should().BeFalse();
        }
    }
}
