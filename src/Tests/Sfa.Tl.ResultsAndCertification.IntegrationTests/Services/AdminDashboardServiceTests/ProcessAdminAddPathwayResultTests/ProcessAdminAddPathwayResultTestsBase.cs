using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessAdminAddPathwayResultTests
{
    public abstract class ProcessAdminAddPathwayResultTestsBase : AdminDashboardServiceBaseTest
    {
        protected int CreateAndSavePathwayAssessment(int registrationPathwayId, DateTime? endDate = null)
        {
            AssessmentSeries assessmentSeries = CreateAssessmentSeries();
            DbContext.Add(assessmentSeries);
            DbContext.SaveChanges();

            TqPathwayAssessment assessment = CreatePathwayAssessment(registrationPathwayId, assessmentSeries.Id, endDate);
            DbContext.Add(assessment);
            DbContext.SaveChanges();

            return assessment.Id;
        }

        protected static AddPathwayResultRequest CreateRequest(int registrationPathwayId, int pathwayAssessmentId)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                PathwayAssessmentId = pathwayAssessmentId,
                SelectedGradeId = 1,
                GradeTo = "A*",
                ContactName = "contact-name",
                RequestDate = new DateTime(2024, 1, 1),
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                CreatedBy = "created-by"
            };
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

        private static TqPathwayAssessment CreatePathwayAssessment(int registrationPathwayId, int assessmentSeriesId, DateTime? endDate)
        {
            return new()
            {
                TqRegistrationPathwayId = registrationPathwayId,
                AssessmentSeriesId = assessmentSeriesId,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = endDate,
                IsOptedin = true,
                IsBulkUpload = false
            };
        }
    }
}