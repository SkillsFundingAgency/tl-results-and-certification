using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.DownloadOverallResultsData
{
    public abstract class TestSetup : BaseTest<DataExportLoader>
    {
        protected IDataExportRepository DataExportRepository;
        protected IBlobStorageService BlobService;
        protected IOverallResultCalculationService OverallResultCalculationService;
        private DataExportLoader _loader;
        protected DataExportResponse Response;

        // params
        protected long ProviderUkprn = 1234567891;
        protected string RequestedBy = "Test User";

        public override void Setup()
        {
            DataExportRepository = Substitute.For<IDataExportRepository>();
            BlobService = Substitute.For<IBlobStorageService>();
            OverallResultCalculationService = Substitute.For<IOverallResultCalculationService>();
        }

        public async override Task When()
        {
            _loader = new DataExportLoader(DataExportRepository, BlobService, OverallResultCalculationService);
            Response = await _loader.DownloadOverallResultsDataAsync(ProviderUkprn, RequestedBy);
        }
    }
}
