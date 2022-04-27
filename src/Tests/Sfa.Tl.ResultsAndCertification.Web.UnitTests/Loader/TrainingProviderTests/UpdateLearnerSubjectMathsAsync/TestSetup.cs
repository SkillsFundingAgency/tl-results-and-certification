using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.UpdateLearnerSubjectMathsAsync
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected AddMathsStatusViewModel AddMathsStatusViewModel { get; set; }
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.UpdateLearnerSubjectAsync(ProviderUkprn, AddMathsStatusViewModel);
        }

        public void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TrainingProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<AddMathsStatusViewModel, UpdateLearnerSubjectRequest>(HttpContextAccessor) :
                                type.Name.Contains("UserEmailResolver") ?
                                (object)new UserEmailResolver<AddMathsStatusViewModel, UpdateLearnerSubjectRequest>(HttpContextAccessor) : null);

            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}