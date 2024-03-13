using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeIndustryPlacementTests
{
    public abstract class ProcessChangeIndustryPlacementTestsBase : AdminDashboardServiceBaseTest
    {
        protected static ReviewChangeIndustryPlacementRequest CreateRequest(
            int registrationPathwayId,
            IndustryPlacementStatus industryPlacementStatus,
            int? hoursSpentOnPlacement = null,
            params int?[] specialConsiderationReasonIds)
        {
            return new()
            {
                RegistrationPathwayId = registrationPathwayId,
                IndustryPlacementStatus = industryPlacementStatus,
                HoursSpentOnPlacement = hoursSpentOnPlacement,
                SpecialConsiderationReasons = specialConsiderationReasonIds.Length > 0 ? specialConsiderationReasonIds.ToList() : null,
                ContactName = "contact-name",
                RequestDate = new DateTime(2024, 1, 1),
                ChangeReason = "change-reason",
                ZendeskId = "1234567890",
                CreatedBy = "created-by"
            };
        }


        protected int CreateAndSaveIndustryPlacement(
            int registrationPathwayId,
            IndustryPlacementStatus industryPlacementStatus,
            int? hoursSpentOnPlacement = null,
            params int?[] specialConsiderationReasonIds)
        {
            string details = industryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration
                ? JsonConvert.SerializeObject(new IndustryPlacementDetails
                {
                    IndustryPlacementStatus = industryPlacementStatus.ToString(),
                    HoursSpentOnPlacement = hoursSpentOnPlacement,
                    SpecialConsiderationReasons = specialConsiderationReasonIds.ToList()
                })
                : null;

            var industryPlacement = new IndustryPlacement
            {
                TqRegistrationPathwayId = registrationPathwayId,
                Status = industryPlacementStatus,
                Details = details
            };

            DbContext.Add(industryPlacement);
            DbContext.SaveChanges();

            return industryPlacement.Id;
        }
    }
}