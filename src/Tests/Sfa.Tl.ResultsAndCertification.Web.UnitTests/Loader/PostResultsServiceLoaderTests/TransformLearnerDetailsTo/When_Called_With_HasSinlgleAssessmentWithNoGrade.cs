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
    public class When_Called_With_HasSinlgleAssessmentWithNoGrade : TestSetup
    {
        public PrsNoGradeRegisteredViewModel ActualResult;

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
                Status = Common.Enum.RegistrationPathwayStatus.Withdrawn,
                PathwayAssessments = new List<PrsAssessment>
                {
                    new PrsAssessment { HasResult = false }
                }
            };
        }

        public override async Task When()
        {
            ActualResult = Loader.TransformLearnerDetailsTo<PrsNoGradeRegisteredViewModel>(FindPrsLearnerRecord);
            await Task.CompletedTask;
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.ProfileId.Should().Be(FindPrsLearnerRecord.ProfileId);
            ActualResult.Uln.Should().Be(FindPrsLearnerRecord.Uln);
            ActualResult.Firstname.Should().Be(FindPrsLearnerRecord.Firstname);
            ActualResult.Lastname.Should().Be(FindPrsLearnerRecord.Lastname);
            ActualResult.DateofBirth.Should().Be(FindPrsLearnerRecord.DateofBirth);
            ActualResult.ProviderName.Should().Be(FindPrsLearnerRecord.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(FindPrsLearnerRecord.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(FindPrsLearnerRecord.TlevelTitle);
            ActualResult.AssessmentSeries.Should().Be(FindPrsLearnerRecord.PathwayAssessments.FirstOrDefault().SeriesName);
        } 
    }
}
