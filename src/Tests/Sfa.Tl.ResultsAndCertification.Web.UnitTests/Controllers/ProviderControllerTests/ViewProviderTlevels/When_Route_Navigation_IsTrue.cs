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
    public class When_Route_Navigation_IsTrue : TestSetup
    {
        private ProviderViewModel mockViewmodel;

        public override void Given()
        {
            navigation = true;

            mockViewmodel = new ProviderViewModel()
            {
                Tlevels = new List<TlevelViewModel>()
                {
                    new TlevelViewModel { TlevelTitle = "Arts", TqProviderId = 1 }
                },
            };

            ProviderLoader.GetViewProviderTlevelViewModelAsync(Arg.Any<long>(), providerId)
                .Returns(mockViewmodel);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            var actualResult = Result.Result;
            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType(typeof(ViewResult));

            var viewResult = actualResult as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ProviderViewModel));

            var resultModel = viewResult.Model as ProviderViewModel;
            resultModel.Should().NotBeNull();
            resultModel.IsNavigatedFromFindProvider.Should().BeTrue();
        }
    }
}
