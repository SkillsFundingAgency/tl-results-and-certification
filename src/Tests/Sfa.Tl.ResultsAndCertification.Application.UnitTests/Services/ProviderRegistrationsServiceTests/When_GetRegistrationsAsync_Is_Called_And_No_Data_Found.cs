using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.ProviderRegistrationsServiceTests
{
    public class When_GetRegistrationsAsync_Is_Called_And_No_Data_Found : ProviderRegistrationsServiceBaseTest
    {
        private readonly long _providerUkprn = 1;
        private readonly int _startYear = 2020;
        private readonly Guid _blobUniqueRef = new("f2e1a7c3-6e4b-4b0d-9e8a-8a2c1e0f3b5e");
        private readonly string _requestedBy = "test-user";

        private DataExportResponse _actualResult;

        public override void Given()
        {
            ProviderRegistrationsRepository.GetRegistrationsAsync(_providerUkprn, _startYear).Returns(new List<TqRegistrationPathway>());
        }

        public override async Task When()
        {
            _actualResult = await ProviderRegistrationsService.GetRegistrationsAsync(_providerUkprn, _startYear, _requestedBy, () => _blobUniqueRef);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.FileSize.Should().Be(0);
            _actualResult.BlobUniqueReference.Should().Be(Guid.Empty);
            _actualResult.ComponentType.Should().Be(ComponentType.NotSpecified);
            _actualResult.IsDataFound.Should().BeFalse();
        }
    }
}