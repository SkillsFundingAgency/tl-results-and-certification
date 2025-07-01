using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeMathsStatusTests
{
    public abstract class ProcessChangeMathsStatusTestsBase : AdminDashboardServiceBaseTest
    {
        protected ReviewChangeMathsStatusRequest Request;
        protected bool Result;

        public override async Task When()
        {
            Result = await AdminDashboardService.ProcessChangeMathsStatusAsync(Request);
        }

        protected ReviewChangeMathsStatusRequest CreateRequest(int registrationPathwayId, SubjectStatus? mathsStatusTo = SubjectStatus.Achieved)
        {
            return new ReviewChangeMathsStatusRequest
            {
                RegistrationPathwayId = registrationPathwayId,
                MathsStatusTo = mathsStatusTo,
                ContactName = "contact-name",
                CreatedBy = "created-by",
                RequestDate = DateTime.Today,
                ChangeReason = "change-reason",
                ZendeskId = "12345"
            };
        }

        protected (int pathwayId, long uln) CreateAndSaveProfileWithMathsStatus(SubjectStatus originalMathsStatus)
        {
            const long uln = 1234567890;

            var profile = new TqRegistrationProfile
            {
                UniqueLearnerNumber = uln,
                Firstname = "Frank",
                Lastname = "West",
                DateofBirth = DateTime.Today.AddYears(-20),
                Gender = "M",
                MathsStatus = originalMathsStatus,
                CreatedBy = "created-by"
            };

            DbContext.Add(profile);
            DbContext.SaveChanges();

            var pathway = new TqRegistrationPathway
            {
                TqRegistrationProfileId = profile.Id,
                AcademicYear = DateTime.Now.Year,
                StartDate = DateTime.Now,
                Status = RegistrationPathwayStatus.Active,
                CreatedBy = "created-by"
            };

            DbContext.Add(pathway);
            DbContext.SaveChanges();

            return (pathway.Id, uln);
        }
    }
}