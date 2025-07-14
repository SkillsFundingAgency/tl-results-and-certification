using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeEnglishStatusTests
{
    public abstract class ProcessChangeEnglishStatusTestsBase : AdminDashboardServiceBaseTest
    {
        protected ReviewChangeEnglishStatusRequest Request;
        protected bool Result;

        public override async Task When()
        {
            Result = await AdminDashboardService.ProcessChangeEnglishStatusAsync(Request);
        }

        protected ReviewChangeEnglishStatusRequest CreateRequest(int registrationPathwayId, SubjectStatus? englishStatusTo = SubjectStatus.Achieved)
        {
            return new ReviewChangeEnglishStatusRequest
            {
                RegistrationPathwayId = registrationPathwayId,
                EnglishStatusTo = englishStatusTo,
                ContactName = "contact-name",
                CreatedBy = "created-by",
                RequestDate = DateTime.Today,
                ChangeReason = "change-reason",
                ZendeskId = "12345"
            };
        }

        protected (int pathwayId, long uln) CreateAndSaveProfileWithEnglishStatus(SubjectStatus originalEnglishStatus)
        {
            const long uln = 1234567890;

            var profile = new TqRegistrationProfile
            {
                UniqueLearnerNumber = uln,
                Firstname = "Frank",
                Lastname = "West",
                DateofBirth = DateTime.Today.AddYears(-20),
                Gender = "M",
                EnglishStatus = originalEnglishStatus,
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