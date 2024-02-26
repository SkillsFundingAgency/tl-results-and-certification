using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResult
{
    public abstract class ProcessAdminAddSpecialismResultTestsBase : AdminDashboardServiceBaseTest
    {
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

        protected static TqSpecialismAssessment CreateSpecialismAssessment(int registrationSpecialismId, int assessmentSeriesId, DateTime? endDate)
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