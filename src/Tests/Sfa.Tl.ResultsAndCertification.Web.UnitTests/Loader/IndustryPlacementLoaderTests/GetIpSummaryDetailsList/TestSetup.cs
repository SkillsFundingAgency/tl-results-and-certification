using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIpSummaryDetailsList
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected IpTempFlexNavigation IpTempFlexNavigation;
        protected IndustryPlacementViewModel CacheModel;
        protected (List<SummaryItemModel>, bool) ActualResult;

        public async override Task When()
        {
            await Task.CompletedTask;
            ActualResult = Loader.GetIpSummaryDetailsListAsync(CacheModel, IpTempFlexNavigation);
        }
    }
}
