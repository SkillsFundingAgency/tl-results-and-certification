using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.LoadAdminAddPathwayResultGrades
{
    public class When_Called : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 125;

        private List<LookupData> _grades;

        private readonly AdminAddPathwayResultViewModel _result = new()
        {
            RegistrationPathwayId = RegistrationPathwayId,
            PathwayAssessmentId = PathwayAssessmentId
        };

        public override void Given()
        {
            _grades = CreatePathwayGrades();
            ApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(_grades);
        }

        public async override Task When()
        {
            await Loader.LoadAdminAddPathwayResultGrades(_result);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Grades.Should().BeEquivalentTo(_grades);
        }
    }
}