using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminChangeLogServiceTests
{
    public abstract class AdminChangeLogServiceBaseTest : BaseTest<AdminChangeLogService>
    {
        protected IAdminChangeLogRepository AdminChangeLogRepository;
        protected AdminChangeLogService AdminChangeLogService;

        public override void Setup()
        {
            AdminChangeLogRepository = Substitute.For<IAdminChangeLogRepository>();
            AdminChangeLogService = new AdminChangeLogService(AdminChangeLogRepository);
        }
    }
}