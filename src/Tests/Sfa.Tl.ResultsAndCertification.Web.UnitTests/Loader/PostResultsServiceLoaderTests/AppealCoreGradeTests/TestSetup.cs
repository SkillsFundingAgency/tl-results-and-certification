using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealCoreGradeTests
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected long AoUkprn;
        protected AppealCoreGradeViewModel AppealCoreGradeViewModel;
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.AppealCoreGradeAsync(AoUkprn, AppealCoreGradeViewModel);
        }
    }
}
