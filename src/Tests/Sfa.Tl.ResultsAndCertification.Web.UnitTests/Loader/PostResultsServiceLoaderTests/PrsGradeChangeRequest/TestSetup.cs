using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsGradeChangeRequest
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected PrsGradeChangeRequestViewModel ViewModel;
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.PrsGradeChangeRequestAsync(ViewModel);
        }
    }
}
