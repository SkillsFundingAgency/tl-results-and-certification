using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddressesByPostcode
{
    public abstract class TestSetup : ProviderAddressLoaderTestBase
    {
        protected string Postcode;
        protected AddAddressSelectViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetAddressesByPostcodeAsync(Postcode);
        }
    }
}
