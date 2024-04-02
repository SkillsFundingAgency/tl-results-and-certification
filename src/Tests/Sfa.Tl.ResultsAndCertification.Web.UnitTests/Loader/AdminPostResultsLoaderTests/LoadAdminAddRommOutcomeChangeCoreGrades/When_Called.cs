using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.LoadAdminAddRommOutcomeChangeCoreGrades
{
    public class When_Called : AdminPostResultsLoaderBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 125;

        private List<LookupData> _grades;

        private readonly AdminAddRommOutcomeChangeGradeCoreViewModel _result = new()
        {
            RegistrationPathwayId = RegistrationPathwayId,
            PathwayAssessmentId = PathwayAssessmentId
        };

        public override void Given()
        {
            _grades = CreatePathwayGrades();
            _grades = _grades.Where(t => !t.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)
               && !t.Code.Equals(Constants.PathwayComponentGradeXNoResultCode, StringComparison.InvariantCultureIgnoreCase)
                && !t.Code.Equals(Constants.NotReceived, StringComparison.InvariantCultureIgnoreCase)
               ).ToList();
            ApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(_grades);
        }

        public async override Task When()
        {
            await Loader.LoadAdminAddRommOutcomeChangeGradeCoreGrades(_result);
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