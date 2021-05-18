using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.AddAddress
{
    public abstract class TestSetup : ProviderAddressLoaderTestBase
    {
        protected long ProviderUkprn;
        protected AddAddressViewModel AddAddressViewModel { get; set; }
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.AddAddressAsync(ProviderUkprn, AddAddressViewModel);
        }

        public void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TrainingProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<AddAddressViewModel, AddAddressRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ?
                                (object)new UserEmailResolver<AddAddressViewModel, AddAddressRequest>(HttpContextAccessor) : null);

            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
