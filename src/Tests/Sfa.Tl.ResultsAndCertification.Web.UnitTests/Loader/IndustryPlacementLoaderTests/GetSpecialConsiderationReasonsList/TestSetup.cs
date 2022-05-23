using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetSpecialConsiderationReasonsList
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected int AcademicYear;
        protected IList<IpLookupDataViewModel> ActualResult;

        public async override Task When()
        {
            ActualResult = await Loader.GetSpecialConsiderationReasonsListAsync(AcademicYear);
        }
    }
}