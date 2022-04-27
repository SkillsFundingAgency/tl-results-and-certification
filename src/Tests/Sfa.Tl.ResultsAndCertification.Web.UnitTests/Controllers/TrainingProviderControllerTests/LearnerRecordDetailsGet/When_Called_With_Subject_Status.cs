using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using System.Collections.Generic;

using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_Subject_Status
    {

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.NotSpecified, EnglishStatus = SubjectStatus.NotSpecified }, SubjectStatusContent.Not_Yet_Recevied_Display_Text },
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.Achieved, EnglishStatus = SubjectStatus.Achieved }, SubjectStatusContent.Achieved_Display_Text },
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.NotAchieved, EnglishStatus = SubjectStatus.NotAchieved }, SubjectStatusContent.Not_Achieved_Display_Text},
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.AchievedByLrs, EnglishStatus = SubjectStatus.AchievedByLrs }, SubjectStatusContent.Achieved_Lrs_Display_Text},
                    new object[] { new LearnerRecordDetailsViewModel1 { MathsStatus = SubjectStatus.NotAchievedByLrs, EnglishStatus = SubjectStatus.NotAchievedByLrs }, SubjectStatusContent.Not_Achieved_Lrs_Display_Text}
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(LearnerRecordDetailsViewModel1 viewModel, string expectedDisplayStatus)
        {
            viewModel.SummaryMathsStatus.Value.Should().Be(expectedDisplayStatus);
            viewModel.SummaryEnglishStatus.Value.Should().Be(expectedDisplayStatus);
        }
    }
}
