using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using System.Collections.Generic;

using LearnerStatusTagContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerStatusTag;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_StatusTag
    {

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.Achieved, EnglishStatus = SubjectStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.Completed}, LearnerStatusTagContent.Tag_Complete },
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.Achieved, EnglishStatus = SubjectStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.NotSpecified }, LearnerStatusTagContent.Tag_Incomplete },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(LearnerRecordDetailsViewModel1 viewModel, string expectedStatusTag)
        {
            viewModel.StatusTag.Should().Be(expectedStatusTag);
        }
    }
}
