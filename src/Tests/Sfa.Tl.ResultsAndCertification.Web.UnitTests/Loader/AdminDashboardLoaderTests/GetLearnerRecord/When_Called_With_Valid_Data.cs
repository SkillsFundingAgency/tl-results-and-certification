using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetLearnerRecord
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        
        private AdminLearnerRecord _apiResult;
        private AdminLearnerRecordViewModel _result;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecord(RegistrationPathwayId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.RegistrationPathwayId.Should().Be(_apiResult.RegistrationPathwayId);
            _result.TlPathwayId.Should().Be(_apiResult.Pathway.Id);
            _result.Uln.Should().Be(_apiResult.Uln);
            _result.LearnerName.Should().Be($"{_apiResult.Firstname} {_apiResult.Lastname}");
            _result.DateofBirth.Should().Be(_apiResult.DateofBirth);
            _result.ProviderName.Should().Be(_apiResult.Pathway.Provider.Name);
            _result.ProviderUkprn.Should().Be(_apiResult.Pathway.Provider.Ukprn);
            _result.TlevelName.Should().Be(_apiResult.Pathway.Name);
            _result.AcademicYear.Should().Be(_apiResult.Pathway.AcademicYear);
            _result.AwardingOrganisationName.Should().Be(_apiResult.AwardingOrganisation.DisplayName);
            _result.MathsStatus.Should().Be(_apiResult.MathsStatus);
            _result.EnglishStatus.Should().Be(_apiResult.EnglishStatus);
            _result.RegistrationPathwayStatus.Should().Be(_apiResult.Pathway.Status);
            _result.IsLearnerRegistered.Should().BeTrue();
            _result.IndustryPlacementId.Should().Be(_apiResult.Pathway.IndustryPlacements.Single().Id);
            _result.IndustryPlacementStatus.Should().Be(_apiResult.Pathway.IndustryPlacements.Single().Status);
        }
    }
}