using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetSpecialConsiderationReasonsList
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private IList<IpLookupData> _mockApiResult = null;

        public override void Given()
        {
            AcademicYear = DateTime.UtcNow.Year;
            InternalApiClient.GetIpLookupDataAsync(Arg.Any<IpLookupType>()).Returns(_mockApiResult);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().BeNull();
        } 
    }
}