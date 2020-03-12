﻿using Xunit;
using NSubstitute;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetAllTlevelsByUkprnAsync
{
    public class Then_ApiClient_Mapper_Is_Called : When_Called_Method_GetTlevelsByUkprnAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().GetAllTlevelsByUkprnAsync(Ukprn);
        }

        [Fact(Skip = "TODO: In progress story")]
        public void Then_Mapper_Is_Called()
        {
            Mapper.Received().Map<IEnumerable<YourTlevelsViewModel>>(ApiClientResponse);
        }

    }
}
