using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminReviewChangesRommOutcommSpecialismTests
{
    public abstract class ProcessAdminReviewChangesRommOutcomeSpecialismBaseTest : AdminPostResultsServiceBaseTest
    {
        protected static ReviewChangesRommOutcomeSpecialismRequest CreateRequest(int registrationPathwayId, int specialismResultId)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                SpecialismResultId = specialismResultId,
                ContactName = "contact-name",
                DateOfRequest = new DateTime(2024, 1, 1),
                ChangeReason = "change-reason",
                ZendeskTicketId = "1234567890",
                CreatedBy = "created-by",
                SelectedGrade = "Merit",
                SelectedGradeId = 11,
                ExistingGrade = "Pass"
            };
        }

        protected TqSpecialismResult CreateAndSaveSpecialismResult(int registrationSpecialismId, int tlLookupId)
        {
            AssessmentSeries assessmentSeries = CreateAssessmentSeries();
            DbContext.Add(assessmentSeries);
            DbContext.SaveChanges();

            TqSpecialismAssessment assessment = CreateSpecialismAssessment(registrationSpecialismId, assessmentSeries.Id);
            DbContext.Add(assessment);
            DbContext.SaveChanges();

            TqSpecialismResult result = CreateSpecialismResult(assessment.Id, tlLookupId);
            DbContext.Add(result);
            DbContext.SaveChanges();

            return result;
        }

        private static AssessmentSeries CreateAssessmentSeries()
        {
            DateTime currentDate = new(2020, 11, 25);

            return new()
            {
                ComponentType = ComponentType.Specialism,
                Name = "Summer 2021",
                Description = "Summer 2021",
                Year = 2021,
                StartDate = currentDate,
                EndDate = currentDate.AddMonths(3),
                RommEndDate = currentDate.AddMonths(4),
                AppealEndDate = currentDate.AddMonths(5),
                ResultCalculationYear = 2020,
                ResultPublishDate = currentDate.AddMonths(6),
                PrintAvailableDate = currentDate.AddMonths(7),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        private static TqSpecialismAssessment CreateSpecialismAssessment(int registrationSpecialismId, int assessmentSeriesId)
            => new()
            {
                TqRegistrationSpecialismId = registrationSpecialismId,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = null,
                IsOptedin = true,
                IsBulkUpload = false,
                AssessmentSeriesId = assessmentSeriesId
            };

        public static TqSpecialismResult CreateSpecialismResult(int specialismAssessmentId, int tlLookupId)
            => new()
            {
                TqSpecialismAssessmentId = specialismAssessmentId,
                TlLookupId = tlLookupId,
                StartDate = new(2021, 1, 10),
                EndDate = null,
                PrsStatus = PrsStatus.UnderReview,
                IsOptedin = true,
                IsBulkUpload = false
            };
    }
}