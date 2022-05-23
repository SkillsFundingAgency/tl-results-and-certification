using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetTemporaryFlexibilities
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected int PathwayId;
        protected int AcademicYear;
        public bool ShowOption;
        protected IList<IpLookupDataViewModel> ActualResult;

        public async override Task When()
        {
            ActualResult = await Loader.GetTemporaryFlexibilitiesAsync(PathwayId, AcademicYear, ShowOption);
        }
    }
}
