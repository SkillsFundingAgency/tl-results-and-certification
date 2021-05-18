using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddressByUprn
{
    public abstract class TestSetup : ProviderAddressLoaderTestBase
    {
        protected long Uprn;
        protected AddressViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetAddressByUprnAsync(Uprn);
        }
    }
}
