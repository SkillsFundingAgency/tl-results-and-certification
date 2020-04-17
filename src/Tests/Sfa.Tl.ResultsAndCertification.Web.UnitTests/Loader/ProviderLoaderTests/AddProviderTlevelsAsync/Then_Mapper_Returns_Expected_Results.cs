using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevelsAsync
{
    public class Then_Mapper_Returns_Expected_Results : When_Called_Method_AddProviderTlevelsAsync
    {    
        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<List<ProviderTlevel>>(ProviderTlevelsViewModel.Tlevels);

            result.Count.Should().Be(2);

            result[0].TqAwardingOrganisationId.Should().Be(ProviderTlevelsViewModel.Tlevels[0].TqAwardingOrganisationId);
            result[0].TlProviderId.Should().Be(ProviderTlevelsViewModel.Tlevels[0].TlProviderId);
            result[0].CreatedBy.Should().Be($"{Givenname} {Surname}");

            result[1].TqAwardingOrganisationId.Should().Be(ProviderTlevelsViewModel.Tlevels[1].TqAwardingOrganisationId);
            result[1].TlProviderId.Should().Be(ProviderTlevelsViewModel.Tlevels[1].TlProviderId);
            result[1].CreatedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
