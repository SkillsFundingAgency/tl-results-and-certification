using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealCoreGrade
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected long AoUkprn;
        protected PrsAddAppealViewModel AppealCoreGradeViewModel;
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.AppealCoreGradeAsync(AoUkprn, AppealCoreGradeViewModel);
        }
    }
}
