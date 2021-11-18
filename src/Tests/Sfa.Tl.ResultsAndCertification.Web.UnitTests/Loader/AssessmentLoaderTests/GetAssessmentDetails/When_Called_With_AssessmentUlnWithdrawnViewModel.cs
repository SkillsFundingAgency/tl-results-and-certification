using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentDetails
{
    public class When_Called_With_AssessmentUlnWithdrawnViewModel : TestSetup
    {
        private AssessmentUlnWithdrawnViewModel _actualResult;

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
                Status = RegistrationPathwayStatus.Active,
                IsIndustryPlacementExist = true,
                IsCoreEntryEligible = true
            };

            InternalApiClient.GetAssessmentDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        public async override Task When()
        {
            _actualResult = await Loader.GetAssessmentDetailsAsync<AssessmentUlnWithdrawnViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();

            _actualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            _actualResult.Uln.Should().Be(expectedApiResult.Uln);
            _actualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            _actualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            _actualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedApiResult.ProviderName);
            _actualResult.ProviderUkprn.Should().Be(expectedApiResult.ProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(expectedApiResult.TlevelTitle);
            _actualResult.ProviderDisplayName.Should().Be($"{expectedApiResult.ProviderName}<br/>({expectedApiResult.ProviderUkprn})");
        }
    }
}
