using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.LoadAdminAddRommOutcomeChangeSpecialismGrades
{
    public class When_Called : AdminPostResultsLoaderBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 125;

        private List<LookupData> _grades;

        private readonly AdminAddRommOutcomeChangeGradeSpecialismViewModel _result = new()
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
            await Loader.LoadAdminAddRommOutcomeChangeGradeSpecialismGrades(_result);
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