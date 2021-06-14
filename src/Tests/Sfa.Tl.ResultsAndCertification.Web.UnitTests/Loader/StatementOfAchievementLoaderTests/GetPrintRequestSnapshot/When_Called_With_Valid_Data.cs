using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetPrintRequestSnapshot
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PrintRequestSnapshot _expectedApiResult;
        private Address _expectedAddress;
        private SoaPrintingDetails _expectedSnapshotDetails;

        public override void Given()
        {
            _expectedAddress = new Address { AddressId = 1, DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };
            _expectedSnapshotDetails = new SoaPrintingDetails
            {
                Uln = 1234567890,
                Name = "First 1 Last 1",
                Dateofbirth = "01 January 2006",
                ProviderName = "Barnsley College (10000536)",

                TlevelTitle = "T Level in Healthcare Science",
                Core = "Healthcare Science (10923456)",
                CoreGrade = "B",
                Specialism = "Optical Care Services (38234567)",
                SpecialismGrade = "None",

                EnglishAndMaths = "Achieved minimum standard (Data from the Learning Records Service - LRS)",
                IndustryPlacement = "Placement completed",
                ProviderAddress = _expectedAddress
            };

            _expectedApiResult = new PrintRequestSnapshot
            {
                RegistrationPathwayStatus = RegistrationPathwayStatus.Withdrawn,
                RequestedBy = "John Smith",
                RequestedOn = DateTime.Today,
                RequestDetails = JsonConvert.SerializeObject(_expectedSnapshotDetails)
            };

            InternalApiClient.GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayStatus.Should().Be(_expectedApiResult.RegistrationPathwayStatus);
            ActualResult.RequestedBy.Should().Be(_expectedApiResult.RequestedBy);
            ActualResult.RequestedOn.Should().Be(_expectedApiResult.RequestedOn);

            var actualSnapshotDetails = ActualResult.SnapshotDetails;
            actualSnapshotDetails.Should().NotBeNull();
            actualSnapshotDetails.Uln.Should().Be(_expectedSnapshotDetails.Uln);
            actualSnapshotDetails.Name.Should().Be(_expectedSnapshotDetails.Name);
            actualSnapshotDetails.Dateofbirth.Should().Be(_expectedSnapshotDetails.Dateofbirth);
            actualSnapshotDetails.ProviderName.Should().Be(_expectedSnapshotDetails.ProviderName);
            actualSnapshotDetails.TlevelTitle.Should().Be(_expectedSnapshotDetails.TlevelTitle);
            actualSnapshotDetails.Core.Should().Be(_expectedSnapshotDetails.Core);
            actualSnapshotDetails.CoreGrade.Should().Be(_expectedSnapshotDetails.CoreGrade);
            actualSnapshotDetails.Specialism.Should().Be(_expectedSnapshotDetails.Specialism);
            actualSnapshotDetails.SpecialismGrade.Should().Be(_expectedSnapshotDetails.SpecialismGrade);
            actualSnapshotDetails.EnglishAndMaths.Should().Be(_expectedSnapshotDetails.EnglishAndMaths);
            actualSnapshotDetails.IndustryPlacement.Should().Be(_expectedSnapshotDetails.IndustryPlacement);

            var actualSnapshotAddress = ActualResult.SnapshotDetails.ProviderAddress;
            actualSnapshotAddress.Should().NotBeNull();
            actualSnapshotAddress.AddressId.Should().Be(_expectedAddress.AddressId);
            actualSnapshotAddress.OrganisationName.Should().Be(_expectedAddress.OrganisationName);
            actualSnapshotAddress.DepartmentName.Should().Be(_expectedAddress.DepartmentName);
            actualSnapshotAddress.AddressLine1.Should().Be(_expectedAddress.AddressLine1);
            actualSnapshotAddress.AddressLine2.Should().Be(_expectedAddress.AddressLine2);
            actualSnapshotAddress.Town.Should().Be(_expectedAddress.Town);
            actualSnapshotAddress.Postcode.Should().Be(_expectedAddress.Postcode);
        }
    }
}