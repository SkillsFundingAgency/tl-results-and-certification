﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProviderChanges
{
    public class When_NoData_Found : TestSetup
    {
        readonly RegistrationDetails mockRegDetails = null;

        public override void Given()
        {
            ViewModel = new ChangeProviderViewModel { ProfileId = 1 };
            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            InternalApiClient.Received(1).GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            ActualResult.Should().BeNull();
        }
    }
}
