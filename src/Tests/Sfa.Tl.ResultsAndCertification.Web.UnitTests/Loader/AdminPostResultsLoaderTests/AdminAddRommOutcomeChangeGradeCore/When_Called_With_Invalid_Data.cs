using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.AdminAddRommOutcomeChangeGradeCore
{
    public class When_Called_With_Invalid_Data : AdminPostResultsLoaderBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 125;

        private List<LookupData> _grades;
        private AdminAddRommOutcomeChangeGradeCoreViewModel _result;

        public override void Given()
        {
            _grades = CreatePathwayGrades();

            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(null as AdminLearnerRecord);
            ApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(_grades);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
            ApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
        }
    }
}