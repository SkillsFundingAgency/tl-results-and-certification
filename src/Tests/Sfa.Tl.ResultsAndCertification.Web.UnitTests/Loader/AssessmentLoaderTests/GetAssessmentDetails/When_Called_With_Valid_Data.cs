using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new AssessmentDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = System.DateTime.UtcNow.AddYears(-29),
                ProviderUkprn = 1234567,
                ProviderName = "Test Provider",
                TlevelTitle = "TLevel in Construction",
                PathwayLarId = "7654321",
                PathwayName = "Pathway",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayAssessmentId = 1,
                SpecialismLarId = "2345678",
                SpecialismName = "Specialism1",
                SpecialismAssessmentSeries = "Autumn 2022",
                SpecialismAssessmentId = 25,
                Specialisms = new List<SpecialismDetails> { new SpecialismDetails { Id = 1, Code = "2345678", Name = "Specialism1" } },
                Status = RegistrationPathwayStatus.Active,
                IsIndustryPlacementExist = true,
                IsCoreEntryEligible = true,
                NextAvailableCoreSeries = "Summer 2021",
                IsSpecialismEntryEligible = true,
                NextAvailableSpecialismSeries = "Summer 2022"
            };

            InternalApiClient.GetAssessmentDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.TlevelTitle);
            ActualResult.Name.Should().Be(string.Concat(expectedApiResult.Firstname, " ", expectedApiResult.Lastname));
            ActualResult.ProviderDisplayName.Should().Be($"{expectedApiResult.ProviderName}<br/>({expectedApiResult.ProviderUkprn})");
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResult.PathwayName} ({expectedApiResult.PathwayLarId})");
            ActualResult.PathwayAssessmentSeries.Should().Be(expectedApiResult.PathwayAssessmentSeries);
            ActualResult.IsResultExist.Should().BeFalse();

            var expectedSpecialismDisplayName = string.Join(Constants.AndSeperator, expectedApiResult.Specialisms.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.Code})"));
            ActualResult.SpecialismDisplayName.Should().Be(expectedSpecialismDisplayName);
            ActualResult.SpecialismAssessmentSeries.Should().Be(expectedApiResult.SpecialismAssessmentSeries);
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Status);
            ActualResult.IsIndustryPlacementExist.Should().BeTrue();
            ActualResult.IsCoreEntryEligible.Should().Be(expectedApiResult.IsCoreEntryEligible);
            ActualResult.NextAvailableCoreSeries.Should().Be(expectedApiResult.NextAvailableCoreSeries);
            ActualResult.IsSpecialismEntryEligible.Should().Be(expectedApiResult.IsSpecialismEntryEligible);
            ActualResult.NextAvailableSpecialismSeries.Should().Be(expectedApiResult.NextAvailableSpecialismSeries);
        }
    }
}
