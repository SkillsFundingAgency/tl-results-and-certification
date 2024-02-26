using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminAddSpecialismResult
{
    public class When_Called_With_Invalid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 777;

        private List<LookupData> _grades;
        private AdminAddSpecialismResultViewModel _result;

        public override void Given()
        {
            _grades = CreateSpecialismGrades();

            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(null as AdminLearnerRecord);
            ApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(_grades);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminAddSpecialismResultAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
            ApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
        }
    }
}