using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AwardingOrganisationLoaderTests
{
    public class Then_ApiClient_Mapper_Is_Called : When_Called_Method_GetTlevelsByAwardingOrganisationAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().GetAllTlevelsByAwardingOrganisationAsync();
        }

        [Fact]
        public void Then_Mapper_Is_Called()
        {
            Mapper.Received().Map<IEnumerable<YourTlevelsViewModel>>(Arg.Any<List<AwardingOrganisationPathwayStatus>>());
        }
    }
}
