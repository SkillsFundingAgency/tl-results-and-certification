using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddress
{
    public abstract class TestSetup : ProviderAddressLoaderTestBase
    {
        protected long ProviderUkprn;
        protected ManagePostalAddressViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetAddressAsync<ManagePostalAddressViewModel>(ProviderUkprn);
        }
    }
}