using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessChangeUln
{
    public abstract class TestSetup : AdminDashboardServiceBaseTest
    {
        protected IRepository<TqRegistrationProfile> Profiles;
        protected IRepository<TqRegistrationPathway> Pathways;
        protected IRepository<ChangeLog> Logs;
        protected TqRegistrationProfile Profile;
        protected ReviewChangeUlnRequest Request;
        protected bool Result;

        public override void Given()
        {
            Profiles = Substitute.For<IRepository<TqRegistrationProfile>>();
            Pathways = Substitute.For<IRepository<TqRegistrationPathway>>();
            Logs = Substitute.For<IRepository<ChangeLog>>();
            Profile = new TqRegistrationProfile { Id = 7, UniqueLearnerNumber = 9999999990 };
            Request = new ReviewChangeUlnRequest
            {
                RegistrationPathwayId = 3, ContactName = "Contact", ChangeReason = "Correction",
                RequestDate = new DateTime(2026, 1, 1), CreatedBy = "Admin",
                ChangeUlnDetails = new ChangeUlnDetails { UlnFrom = 9999999990, UlnTo = 9999999991 }
            };
            RepositoryFactory.GetRepository<TqRegistrationProfile>().Returns(Profiles);
            RepositoryFactory.GetRepository<TqRegistrationPathway>().Returns(Pathways);
            RepositoryFactory.GetRepository<ChangeLog>().Returns(Logs);
            Pathways.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<TqRegistrationPathway, bool>>>(),
                Arg.Any<Expression<Func<TqRegistrationPathway, object>>[]>())
                .Returns(new TqRegistrationPathway { Id = 3, TqRegistrationProfileId = 7, Status = RegistrationPathwayStatus.Withdrawn, AcademicYear = 2020 });
            Profiles.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<TqRegistrationProfile, bool>>>(),
                Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>()).Returns(Profile);
            Profiles.UpdateWithSpecifedColumnsOnlyAsync(Profile, Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>()).Returns(1);
            Logs.CreateAsync(Arg.Any<ChangeLog>()).Returns(1);
        }

        public override async Task When()
        {
            Result = await AdminDashboardService.ProcessChangeUlnAsync(Request);
        }
    }
}
