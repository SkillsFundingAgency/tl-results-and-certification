using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.IndustryPlacementExtractTests
{
    public abstract class IndustryPlacementExtractFunctionTestBase : BaseTest<IndustryPlacementExtract>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementService IndustryPlacementService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementExtract IndustryPlacementExtractFunction;

        //protected IIndustryPlacementService IndustryPlacementService;
        //protected IBlobStorageService BlobStorageService;
        //protected IUcasApiClient UcasApiClient;
        //protected IRepository<IpLookup> IpLookupRepository;
        //protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;
        //protected ILogger<GenericRepository<TqRegistrationPathway>> RegistrationPathwayRepositoryLogger;
        //protected ICommonRepository CommonRepository;

        //protected ILogger<GenericRepository<IndustryPlacement>> IndustryPlacementLogger;
        //protected IRepository<IndustryPlacement> IndustryPlacementRepository;
        //protected ResultsAndCertificationDbContext DbContext;
        //protected ILogger<GenericRepository<IpLookup>> IpLookupRepositoryLogger;


        //protected ILogger<IIndustryPlacementService> Logger;
        //protected UcasRecordEntriesSegment UcasRecordEntrySegment;

        //// Actual test instance
        //protected Application.Services.IndustryPlacementService Service;

        //// Result
        //protected FunctionResponse ActualResult;


        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementService = Substitute.For<IIndustryPlacementService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementExtractFunction = new IndustryPlacementExtract(IndustryPlacementService, CommonService);
        }

        //public override void Setup()
        //{
        //    IpLookupRepositoryLogger = new Logger<GenericRepository<IpLookup>>(new NullLoggerFactory());
        //    IpLookupRepository = new GenericRepository<IpLookup>(IpLookupRepositoryLogger, DbContext);

        //    IndustryPlacementLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
        //    IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementLogger, DbContext);

        //    RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
        //    RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

        //    CommonRepository = new CommonRepository(DbContext);


        //    BlobStorageService = Substitute.For<IBlobStorageService>();
        //    Logger = Substitute.For<ILogger<IIndustryPlacementService>>();

        //    //Service = new IndustryPlacementService(IpLookupRepository, IndustryPlacementRepository, 
        //    //    RegistrationPathwayRepository, CommonRepository, BlobStorageService, mapper,  Logger);
        //}
    }
}
