using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddPathwayResultTests
{
    public abstract class ProcessAdminAddPathwayResultTestsBase : AdminDashboardServiceBaseTest
    {
        protected static AddPathwayResultRequest CreateRequest(int registrationPathwayId, int pathwayAssessmentId)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                PathwayAssessmentId = pathwayAssessmentId,
                SelectedGradeId = 1,
                ContactName = "contact-name",
                RequestDate = new DateTime(2024, 1, 1),
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                CreatedBy = "created-by"
            };
        }

        protected static TqPathwayAssessment CreatePathwayAssessment(int registrationPathwayId, int assessmentSeriesId, DateTime? endDate)
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