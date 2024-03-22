using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminOpenPathwayRommTests
{
    public abstract class ProcessAdminOpenPathwayRommBaseTest : AdminPostResultsServiceBaseTest
    {
        protected static OpenPathwayRommRequest CreateRequest(int registrationPathwayId, int pathwayResultId)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                PathwayResultId = pathwayResultId,
                ContactName = "contact-name",
                DateOfRequest = new DateTime(2024, 1, 1),
                ChangeReason = "change-reason",
                ZendeskTicketId = "1234567890",
                CreatedBy = "created-by"
            };
        }

        protected TqPathwayResult CreateAndSavePathwayResult(int registrationPathwayId, int tlLookupId)
        {
            AssessmentSeries assessmentSeries = CreateAssessmentSeries();
            DbContext.Add(assessmentSeries);
            DbContext.SaveChanges();

            TqPathwayAssessment assessment = CreatePathwayAssessment(registrationPathwayId, assessmentSeries.Id);
            DbContext.Add(assessment);
            DbContext.SaveChanges();

            TqPathwayResult result = CreatePathwayResult(assessment.Id, tlLookupId);
            DbContext.Add(result);
            DbContext.SaveChanges();

            return result;
        }

        private static AssessmentSeries CreateAssessmentSeries()
        {
            DateTime currentDate = new(2020, 11, 25);

            return new()
            {
                ComponentType = ComponentType.Core,
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

        private static TqPathwayAssessment CreatePathwayAssessment(int registrationPathwayId, int assessmentSeriesId)
            => new()
            {
                TqRegistrationPathwayId = registrationPathwayId,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = null,
                IsOptedin = true,
                IsBulkUpload = false,
                AssessmentSeriesId = assessmentSeriesId
            };

        private static TqPathwayResult CreatePathwayResult(int pathwayAssessmentId, int tlLookupId)
            => new()
            {
                TqPathwayAssessmentId = pathwayAssessmentId,
                TlLookupId = tlLookupId,
                StartDate = new(2021, 1, 10),
                EndDate = null,
                PrsStatus = null,
                IsOptedin = true,
                IsBulkUpload = false
            };
    }
}