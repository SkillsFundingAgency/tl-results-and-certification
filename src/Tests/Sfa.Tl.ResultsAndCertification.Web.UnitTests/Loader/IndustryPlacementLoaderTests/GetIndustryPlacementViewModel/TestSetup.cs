using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIndustryPlacementViewModel
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected int PathwayId;

        public IndustryPlacementViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetIndustryPlacementViewModelAsync(ProviderUkprn, ProfileId);
        }
    }
}
