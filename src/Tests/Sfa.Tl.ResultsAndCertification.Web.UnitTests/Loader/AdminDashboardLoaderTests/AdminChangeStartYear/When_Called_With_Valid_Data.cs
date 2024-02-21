using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminChangeStartYear
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;

        private AdminLearnerRecord _apiResult;
        private AdminChangeStartYearViewModel _result;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecord(RegistrationPathwayId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(RegistrationPathwayId);
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
            _result.PathwayId.Should().Be(_apiResult.Pathway.Id);
            _result.FirstName.Should().Be(_apiResult.Firstname);
            _result.LastName.Should().Be(_apiResult.Lastname);
            _result.Uln.Should().Be(_apiResult.Uln);
            _result.ProviderName.Should().Be(_apiResult.Pathway.Provider.Name);
            _result.ProviderUkprn.Should().Be(_apiResult.Pathway.Provider.Ukprn);
            _result.TlevelName.Should().Be(_apiResult.Pathway.Name);
            _result.TlevelStartYear.Should().Be(_apiResult.Pathway.StartYear);
            _result.AcademicYear.Should().Be(_apiResult.Pathway.AcademicYear);
            _result.DisplayAcademicYear.Should().Be($"{_apiResult.Pathway.AcademicYear} to {_apiResult.Pathway.AcademicYear + 1}");
            _result.LearnerRegistrationPathwayStatus.Should().Be(_apiResult.Pathway.Status.ToString());
            _result.OverallCalculationStatus.Should().Be(_apiResult.OverallCalculationStatus);
            _result.AcademicStartYearsToBe.Should().BeEquivalentTo(new[] { 2022, 2021 });
        }
    }
}