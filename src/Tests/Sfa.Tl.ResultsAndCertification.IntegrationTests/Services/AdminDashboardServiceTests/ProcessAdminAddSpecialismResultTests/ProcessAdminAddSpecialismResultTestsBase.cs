using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResultTests
{
    public abstract class ProcessAdminAddSpecialismResultTestsBase : AdminDashboardServiceBaseTest
    {
        protected int CreateAndSaveSpecialismAssessment(int registrationSpecialismId, DateTime? endDate = null)
        {
            AssessmentSeries assessmentSeries = CreateAssessmentSeries();
            DbContext.Add(assessmentSeries);
            DbContext.SaveChanges();

            TqSpecialismAssessment assessment = CreateSpecialismAssessment(registrationSpecialismId, assessmentSeries.Id, endDate);
            DbContext.Add(assessment);
            DbContext.SaveChanges();

            return assessment.Id;
        }

        protected static AddSpecialismResultRequest CreateRequest(int registrationPathwayId, int specialismAssessmentId)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                SpecialismAssessmentId = specialismAssessmentId,
                SelectedGradeId = 1,
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

        private static TqSpecialismAssessment CreateSpecialismAssessment(int registrationSpecialismId, int assessmentSeriesId, DateTime? endDate)
        {
            return new()
            {
                TqRegistrationSpecialismId = registrationSpecialismId,
                AssessmentSeriesId = assessmentSeriesId,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = endDate,
                IsOptedin = true,
                IsBulkUpload = false
            };
        }
    }
}