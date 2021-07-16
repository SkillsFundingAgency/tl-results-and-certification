using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.TransformLearnerDetailsTo
{
    public class When_Called_With_PrsSelectAssessmentSeriesViewModel : TestSetup
    {
        public PrsSelectAssessmentSeriesViewModel ActualResult;

        public override void Given()
        {
            FindPrsLearnerRecord = new Models.Contracts.PostResultsService.FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Provider",
                ProviderUkprn = 7894561231,
                TlevelTitle = "Title",
                Status = Common.Enum.RegistrationPathwayStatus.Active,
                PathwayAssessments = new List<PrsAssessment>
                {
                    new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                    new PrsAssessment { AssessmentId = 22, SeriesName = "Autumn 2021", HasResult = false }
                }
            };
        }

        public override async Task When()
        {
            ActualResult = Loader.TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(FindPrsLearnerRecord);
            await Task.CompletedTask;
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Uln.Should().Be(FindPrsLearnerRecord.Uln);
            ActualResult.Firstname.Should().Be(FindPrsLearnerRecord.Firstname);
            ActualResult.Lastname.Should().Be(FindPrsLearnerRecord.Lastname);
            ActualResult.DateofBirth.Should().Be(FindPrsLearnerRecord.DateofBirth);
            ActualResult.ProviderName.Should().Be(FindPrsLearnerRecord.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(FindPrsLearnerRecord.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(FindPrsLearnerRecord.TlevelTitle);
            ActualResult.SelectedAssessmentId.Should().BeNull();
            ActualResult.AssessmentSerieses.Count().Should().Be(FindPrsLearnerRecord.PathwayAssessments.Count());

            var expectedAssessment = FindPrsLearnerRecord.PathwayAssessments.ToList();
            foreach (var actual in ActualResult.AssessmentSerieses.Select((value, i) => (value, i)))
            {
                actual.value.AssessmentId.Should().Be(expectedAssessment[actual.i].AssessmentId);
                actual.value.SeriesName.Should().Be(expectedAssessment[actual.i].SeriesName);
                actual.value.HasResult.Should().Be(expectedAssessment[actual.i].HasResult);
            }
        }
    }
}