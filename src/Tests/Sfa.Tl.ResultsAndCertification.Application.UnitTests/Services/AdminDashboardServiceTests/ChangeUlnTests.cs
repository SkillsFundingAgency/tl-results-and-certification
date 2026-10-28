using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class ChangeUlnTests
    {
        private readonly IRepository<TqRegistrationProfile> _profiles = Substitute.For<IRepository<TqRegistrationProfile>>();
        private readonly IRepository<TqRegistrationPathway> _pathways = Substitute.For<IRepository<TqRegistrationPathway>>();
        private readonly IRepository<ChangeLog> _logs = Substitute.For<IRepository<ChangeLog>>();
        private readonly AdminDashboardService _service;
        private readonly TqRegistrationProfile _profile = new() { Id = 7, UniqueLearnerNumber = 9999999990 };
        private readonly ReviewChangeUlnRequest _request = new()
        {
            RegistrationPathwayId = 3, ContactName = "Contact", ChangeReason = "Correction",
            RequestDate = new DateTime(2026, 1, 1), CreatedBy = "Admin",
            ChangeUlnDetails = new() { UlnFrom = 9999999990, UlnTo = 9999999991 }
        };

        public ChangeUlnTests()
        {
            var factory = Substitute.For<IRepositoryFactory>();
            factory.GetRepository<TqRegistrationProfile>().Returns(_profiles);
            factory.GetRepository<TqRegistrationPathway>().Returns(_pathways);
            factory.GetRepository<ChangeLog>().Returns(_logs);
            _pathways.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<TqRegistrationPathway, bool>>>(),
                Arg.Any<Expression<Func<TqRegistrationPathway, object>>[]>())
                .Returns(new TqRegistrationPathway { Id = 3, TqRegistrationProfileId = 7, Status = RegistrationPathwayStatus.Withdrawn, AcademicYear = 2020 });
            _profiles.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<TqRegistrationProfile, bool>>>(),
                Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>()).Returns(_profile);
            _profiles.UpdateWithSpecifedColumnsOnlyAsync(_profile, Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>()).Returns(1);
            _logs.CreateAsync(Arg.Any<ChangeLog>()).Returns(1);
            _service = new AdminDashboardService(Substitute.For<IAdminDashboardRepository>(), factory,
                Substitute.For<ISystemProvider>(), Substitute.For<IMapper>());
        }

        [Fact]
        public async Task Changes_shared_profile_and_logs_before_and_after_for_old_withdrawn_learner()
        {
            Assert.True(await _service.ProcessChangeUlnAsync(_request));
            Assert.Equal(9999999991, _profile.UniqueLearnerNumber);
            await _logs.Received(1).CreateAsync(Arg.Is<ChangeLog>(l => l.ChangeType == ChangeType.Uln
                && l.TqRegistrationPathwayId == 3 && l.CreatedBy == "Admin"
                && l.Details.Contains("9999999990") && l.Details.Contains("9999999991")));
            await _pathways.DidNotReceive().UpdateAsync(Arg.Any<TqRegistrationPathway>());
        }

        [Fact]
        public async Task Rejects_duplicate_without_updating_or_logging()
        {
            _profiles.AnyAsync(Arg.Any<Expression<Func<TqRegistrationProfile, bool>>>()).Returns(true);
            Assert.False(await _service.ProcessChangeUlnAsync(_request));
            Assert.Equal(9999999990, _profile.UniqueLearnerNumber);
            await _logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }

        [Fact]
        public async Task Rejects_stale_review()
        {
            _profile.UniqueLearnerNumber = 8888888888;
            Assert.False(await _service.ProcessChangeUlnAsync(_request));
            await _logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }

        [Fact]
        public async Task Same_uln_does_not_create_a_change_log()
        {
            _request.ChangeUlnDetails.UlnTo = _request.ChangeUlnDetails.UlnFrom;
            Assert.True(await _service.ProcessChangeUlnAsync(_request));
            await _logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }

        [Fact]
        public async Task Failed_profile_update_does_not_create_a_change_log()
        {
            _profiles.UpdateWithSpecifedColumnsOnlyAsync(_profile,
                Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>()).Returns(0);

            Assert.False(await _service.ProcessChangeUlnAsync(_request));
            await _logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }

        [Fact]
        public async Task Audit_failure_does_not_report_success()
        {
            _logs.CreateAsync(Arg.Any<ChangeLog>()).Returns(0);
            Assert.False(await _service.ProcessChangeUlnAsync(_request));
        }
    }
}
