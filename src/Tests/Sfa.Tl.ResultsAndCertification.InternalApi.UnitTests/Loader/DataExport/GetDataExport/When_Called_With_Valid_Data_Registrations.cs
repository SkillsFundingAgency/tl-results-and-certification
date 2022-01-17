using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.GetDataExport
{
    public class When_Called_With_Valid_Data_Registrations : TestSetup
    {
        private IList<RegistrationsExport> _registrations;

        public override void Given()
        {
            DataExportType = Common.Enum.DataExportType.Registrations;

            _registrations = new List<RegistrationsExport> { new RegistrationsExport { FirstName = "John", LastName = "Smith" } };
            DataExportService.GetDataExportRegistrationsAsync(AoUkprn).Returns(_registrations);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Response.Should().NotBeNull();
            Response.Count.Should().Be(1);
            Response.FirstOrDefault().IsDataFound.Should().BeTrue();
            Response.FirstOrDefault().ComponentType.Should().Be(Common.Enum.ComponentType.NotSpecified);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            DataExportService.Received(1).GetDataExportRegistrationsAsync(AoUkprn);
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
