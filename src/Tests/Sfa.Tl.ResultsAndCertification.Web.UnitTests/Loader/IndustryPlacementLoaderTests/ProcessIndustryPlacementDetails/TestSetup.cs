using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessIndustryPlacementDetails
{
    public abstract class TestSetup : IndustryPlacementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected bool ActualResult { get; set; }
        protected IndustryPlacementViewModel ViewModel { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessIndustryPlacementDetailsAsync(ProviderUkprn, ViewModel);
        }

        public virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(IndustryPlacementMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<IndustryPlacementViewModel, IndustryPlacementRequest>(HttpContextAccessor) :
                                null);

            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
