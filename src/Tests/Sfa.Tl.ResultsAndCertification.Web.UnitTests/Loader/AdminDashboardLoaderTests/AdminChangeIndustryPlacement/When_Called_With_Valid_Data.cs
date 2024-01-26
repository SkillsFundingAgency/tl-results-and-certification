using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminChangeIndustryPlacement
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;

        private AdminLearnerRecord _apiResult;
        private AdminIpCompletionViewModel _result;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecord(RegistrationPathwayId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
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
            _result.LearnerName.Should().Be($"{_apiResult.Firstname} {_apiResult.Lastname}");
            _result.Uln.Should().Be(_apiResult.Uln);
            _result.Provider.Should().Be($"{_apiResult.Pathway.Provider.Name} ({_apiResult.Pathway.Provider.Ukprn})");
            _result.TlevelName.Should().Be(_apiResult.Pathway.Name);
            _result.AcademicYear.Should().Be(_apiResult.Pathway.AcademicYear);
            _result.StartYear.Should().Be($"{_apiResult.Pathway.AcademicYear} to {_apiResult.Pathway.AcademicYear + 1}");

            IndustryPlacementStatus industryPlacementStatus = _apiResult.Pathway.IndustryPlacements.Single().Status;
            _result.IndustryPlacementStatus.Should().Be(industryPlacementStatus);
            _result.IndustryPlacementStatusTo.Should().Be(industryPlacementStatus);
        }
    }
}