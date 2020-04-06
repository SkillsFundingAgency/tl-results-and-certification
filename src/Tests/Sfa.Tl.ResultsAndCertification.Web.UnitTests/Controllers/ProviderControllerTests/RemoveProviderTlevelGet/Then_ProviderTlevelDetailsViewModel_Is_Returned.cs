using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelGet
{
    public class Then_ProviderTlevelDetailsViewModel_Is_Returned : When_RemoveProviderTlevelAsync_Get_Action_Is_Called
    {
        private ProviderTlevelDetailsViewModel mockresult;
        public override void Given()
        {
            Ukprn = 10011881;
            TqProviderId = 1;

            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            mockresult = new ProviderTlevelDetailsViewModel
            {
                Id = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                TlevelTitle = "Test Title"                
            };

            ProviderLoader.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId).Returns(mockresult);
        }

        [Fact]
        public void Then_GetTqProviderTlevelDetailsAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId);
        }

        [Fact]
        public void Then_Expeced_Results_Returned()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ProviderTlevelDetailsViewModel));

            var model = viewResult.Model as ProviderTlevelDetailsViewModel;
            model.Should().NotBeNull();
            model.Id.Should().Be(mockresult.Id);
            model.DisplayName.Should().Be(mockresult.DisplayName);
            model.Ukprn.Should().Be(mockresult.Ukprn);
            model.TlevelTitle.Should().Be(mockresult.TlevelTitle);
        }
    }
}
