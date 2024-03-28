using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.LoadAdminAddRommOutcomeSpecialismGrades
{
    public class When_Called : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 125;

        private List<LookupData> _grades;

        private readonly AdminAddRommOutcomeSpecialismViewModel _result = new()
        {
            RegistrationPathwayId = RegistrationPathwayId,
            SpecialismAssessmentId = SpecialismAssessmentId
        };

        public override void Given()
        {
            _grades = CreatePathwayGrades();
            _grades = _grades.Where(t => !t.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.SpecialismComponentGradeXNoResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.NotReceived, StringComparison.InvariantCultureIgnoreCase)).ToList();
            ApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(_grades);          
        }

        public async override Task When()
        {
            await Loader.LoadAdminAddRommOutcomeSpecialismGrades(_result);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Grades.Should().BeEquivalentTo(_grades);
        }
    }
}