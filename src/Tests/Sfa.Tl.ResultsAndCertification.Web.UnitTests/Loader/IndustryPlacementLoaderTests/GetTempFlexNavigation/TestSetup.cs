using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetTempFlexNavigation
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected int PathwayId;
        protected int AcademicYear;
        protected IpTempFlexNavigation ActualResult;

        public async override Task When()
        {
            ActualResult = await Loader.GetTempFlexNavigationAsync(PathwayId, AcademicYear);
        }
    }
}
