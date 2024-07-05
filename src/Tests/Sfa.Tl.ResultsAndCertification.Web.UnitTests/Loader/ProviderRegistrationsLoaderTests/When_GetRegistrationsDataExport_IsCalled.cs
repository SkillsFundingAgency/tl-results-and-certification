using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderRegistrationsLoaderTests
{
    public class When_GetRegistrationsDataExport_IsCalled : ProviderRegistrationsLoaderBaseTest
    {
        private const long ProviderUkprn = 1;
        private const int StartYear = 2020;
        private const string RequestedBy = "test-user";

        private DataExportResponse _expectedResult;
        private DataExportResponse _actualResult;

        public override void Given()
        {
            _expectedResult = new DataExportResponse
            {
                IsDataFound = true,
                BlobUniqueReference = new Guid("b7a8d6e1-3c0e-4f1b-9c5a-2e1e1b4c8e6d"),
                FileSize = 100,
                ComponentType = Common.Enum.ComponentType.NotSpecified
            };

            ApiClient.GetProviderRegistrationsAsync(Arg.Is<GetProviderRegistrationsRequest>(r => 
                r.ProviderUkprn == ProviderUkprn
                && r.StartYear == StartYear
                && r.RequestedBy == RequestedBy))
            .Returns(_expectedResult);
        }

        public override async Task When()
        {
            _actualResult = await Loader.GetRegistrationsDataExportAsync(ProviderUkprn, StartYear, RequestedBy);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}