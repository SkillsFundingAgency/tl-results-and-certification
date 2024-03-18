using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenPathwayRomm
{
    public abstract class TestSetup : AdminPostResultsLoaderBaseTest
    {
        protected const int RegistrationPathwayId = 1;
        protected const int PathwayAssessmentId = 125;

        protected AdminOpenPathwayRommViewModel Result { get; private set; }

        public override async Task When()
        {
            Result = await Loader.GetAdminOpenPathwayRommAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        protected AdminLearnerRecord CreateAdminLearnerRecordWithPathwayAssessment(int registrationPathwayId, int pathwayAssessmentId)
        {
            var learnerRecord = CreateAdminLearnerRecord(registrationPathwayId);

            learnerRecord.Pathway.PathwayAssessments = new Assessment[]
            {
                new Assessment
                {
                    Id = pathwayAssessmentId,
                    SeriesId = 1,
                    SeriesName = "Autum 2023",
                    ResultEndDate = new DateTime(2024, 1, 1),
                    RommEndDate = new DateTime(2024, 2, 1),
                    AppealEndDate = new DateTime(2024, 3, 1),
                    LastUpdatedOn = new DateTime(2023, 9, 15),
                    LastUpdatedBy = "test-user",
                    ComponentType = ComponentType.Core,
                    Result = new Result
                    {
                        Id = 1,
                        Grade = "A",
                        GradeCode = "PCG2",
                        PrsStatus = PrsStatus.NotSpecified,
                        LastUpdatedOn = new DateTime(2023, 9, 15),
                        LastUpdatedBy = "test-user"
                    }
                }
            };

            return learnerRecord;
        }
    }
}
